//insert block ui

function BlockUI(elementID) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function () {
        $("#" + elementID).block({
            message: '<table><tr><td>' + '<img src="../images/ajax-loader.gif"/></td></tr></table>',
            css: {},
            overlayCSS: { backgroundColor: '#FFFFFF', opacity: 0.6, border: '1px solid #000000' }
        });
    });
    prm.add_endRequest(function () {
        $("#" + elementID).unblock();
    });
}
