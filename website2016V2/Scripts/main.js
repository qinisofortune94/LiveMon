//comment
var maintree;
function initialize() {

}

function updatePage(tree) {
    /// <summary>
    /// Updates the page.
    /// </summary>
    /// <param name="tree">The tree.</param>
    /// <returns></returns>
    var info_panel = document.getElementById("information_panel");
    var header = document.getElementById("header");
    header.innerHTML = "Metering Layout";
    var info_panel_html = "<ul>";
    info_panel_html += "<li>Meter: " + tree.text + "</li>";
    info_panel_html += "<li>Sensor ID: " + tree.treeData.SensorID + "</li>";
    info_panel_html += "</ul>";
    info_panel.innerHTML = info_panel_html;
}


function callAjax_GetAllEquipmentLayout() {
   // debugger;
    /// <summary>
    /// Calls the ajax FindEquipmentLayoutTree get all equipment layout.
    /// </summary>
    /// <returns> List/array of Json Heirachy objects</returns>
    $.ajax(
        {
            type: "POST",
            url: "/EquipmentLayout.svc/FindEquipmentLayoutTree",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccess,
            failure: function (response) {

                alert("error loading Layout data");
            }
        });
}
function callAjax_GetspecificEquipmentLayout(RootID) {
   
    $.ajax(
        {
            type: "POST",
            url: "/EquipmentLayout.svc/FindEquipmentLayoutTreeByID",
            data: JSON.stringify({ sensorid: RootID }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccess,
            failure: function (response) {

                alert("error loading Layout data");
            }
        });
}
function OnSuccess(response) {
   // debugger;
    /// <summary>
    /// Called when [success].
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>void</returns>
    document.getElementById("canvas").innerHTML = "";
    buildTree(response.d);

}

function buildTree(EquipmentLayoutTreeData) {
    /// <summary>
    /// Builds the tree. using the heirachy
    /// </summary>
    /// <param name="EquipmentLayoutTreeData">The equipment layout tree data.</param>
    /// <returns></returns>
    debugger;//stop in browser scriot debugger
    if (EquipmentLayoutTreeData.length >= 0) {//only ifthere is data
        var canvas = document.getElementById("canvas"),
       context = canvas.getContext("2d"),
       tree = TREE.create(EquipmentLayoutTreeData[0].SensorName),
       nodes = TREE.getNodeList(tree),
       currNode = tree,
       zoom_in = document.getElementById("zoom_in"),
       zoom_out = document.getElementById("zoom_out");
        var descendant = 1;
        //add_child_button = document.getElementById("add_child"),
        //remove_node = document.getElementById("remove_node"),
        tree.treeData = EquipmentLayoutTreeData[0];


        canvas.addEventListener("click", function (event) {
            var x = event.pageX - canvas.offsetLeft,
                y = event.pageY - canvas.offsetTop;
            for (var i = 0; i < nodes.length; i++) {
                if (x > nodes[i].xPos && y > nodes[i].yPos && x < nodes[i].xPos + nodes[i].width && y < nodes[i].yPos + nodes[i].height) {
                    currNode.selected(false);
                    nodes[i].selected(true);
                    currNode = nodes[i];
                    TREE.clear(context);
                    TREE.draw(context, tree);
                    updatePage(currNode);
                    break;
                }
            }
        }, false);

        canvas.addEventListener("mousemove", function (event) {
            var x = event.pageX - canvas.offsetLeft,
                y = event.pageY - canvas.offsetTop;
            for (var i = 0; i < nodes.length; i++) {
                if (x > nodes[i].xPos && y > nodes[i].yPos && x < nodes[i].xPos + nodes[i].width && y < nodes[i].yPos + nodes[i].height) {
                    canvas.style.cursor = "pointer";
                    break;
                }
                else {
                    canvas.style.cursor = "auto";
                }
            }
        }, false);
        //add_child_button.addEventListener('click', function (event) {
        //    currNode.addChild(TREE.create("Child of " + currNode.text));
        //    TREE.clear(context);
        //    nodes = TREE.getNodeList(tree);
        //    TREE.draw(context, tree);
        //}, false);
        //remove_node.addEventListener('click', function (event) {
        //    TREE.destroy(currNode);
        //    TREE.clear(context);
        //    nodes = TREE.getNodeList(tree);
        //    TREE.draw(context, tree);
        //}, false);
        zoom_in.addEventListener('click', function (event) {
            for (var i = 0; i < nodes.length; i++) {
                nodes[i].width *= 1.05;
                nodes[i].height *= 1.05;
            }
            TREE.config.width *= 1.05;
            TREE.config.height *= 1.05;
            TREE.clear(context);
            TREE.draw(context, tree);
        }, false);
        zoom_out.addEventListener('click', function (event) {
            for (var i = 0; i < nodes.length; i++) {
                nodes[i].width = nodes[i].width * 0.95;
                nodes[i].height = nodes[i].height * 0.95;
            }
            TREE.config.width *= 0.95;
            TREE.config.height *= 0.95;
            TREE.clear(context);
            TREE.draw(context, tree);
        }, false);
        context.canvas.width = document.getElementById("main").offsetWidth;
        context.canvas.height = document.getElementById("main").offsetHeight;


        //for (var index = 0; index < EquipmentLayoutTreeData.length; index++) {

        //tree.addChild(TREE.create(EquipmentLayoutTreeData[index].SensorName));


        //add the children of the root object
        if (EquipmentLayoutTreeData[0].HasChildren) {
            addChildren(EquipmentLayoutTreeData[0].Children, tree, 0, 1, descendant)
        }
        // }
        try {
            sessionStorage.setItem("EquipmentLayoutTreeData", JSON.stringify(EquipmentLayoutTreeData));
        }
        catch (err) {
            debugger;
        }
        //End change
        nodes = TREE.getNodeList(tree);
        TREE.draw(context, tree);
        maintree = tree;

        //Adjusting the main container to the number of available machines
        tree.selected(true);
        updatePage(tree);
    }


}
function addChildren(EquipmentChildren, tree, indexno, level,newdescendant) {
    /// <summary>
    /// Adds the children.
    /// </summary>
    /// <param name="EquipmentChildren">The equipment children.</param>
    /// <param name="tree">The tree.</param>
    /// <param name="indexno">The indexno.</param>
    /// <param name="level">The level.</param>
    /// <param name="newdescendant">The newdescendant.</param>
    /// <returns></returns>
    debugger;
    for (var index = 0; index < EquipmentChildren.length; index++) {
        try {
            if (level == 1)//first level add child
            {
                var newtree = TREE.create(EquipmentChildren[index].SensorName);
                tree.addChild(newtree);
                newtree.treeData = EquipmentChildren[index];
                newdescendant = newdescendant + 1;
                if (EquipmentChildren[index].HasChildren) {
                    addChildren(EquipmentChildren[index].Children, tree, index, 2, newdescendant)
                }
            }
            if (level == 2)//second level add descendants of child and recurse
            {
                var newtree2 = TREE.create(EquipmentChildren[index].SensorName);
                tree.getDescendent(newdescendant).addChild(newtree2);
                newtree2.treeData = EquipmentChildren[index];
                //tree.getChildAt(indexno).addChild(TREE.create(EquipmentChildren[index].SensorName));
                if (EquipmentChildren[index].HasChildren) {
                    addChildren(EquipmentChildren[index].Children, tree, index, 2, newdescendant + 1)
                }
                //newdescendant = newdescendant + 1;
            }
           
        }
        catch (err) {
            debugger;
        }
        
    }
}