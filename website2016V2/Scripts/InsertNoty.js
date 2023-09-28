function Shownoty(message) {
    var n = noty({
        layout: 'topLeft',
        text: message,
        type: 'error',
        dismissQueue: true,
        theme: 'defaultTheme',
        maxVisible: 10
    });
    //console.log('html: ' + n.options.id);
}
//call page behind
//System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "ShownotyBehindScript", "ShownotyBehind('From page behind" + Now.ToString + "','warning','topRight','defaultTheme');", True)

function ShownotyBehind(message, notytype, notylayout, notyTheme) {
    var n = noty({
        layout: notylayout,
        text: message,
        type: notytype,
        dismissQueue: true,
        theme: notyTheme,
        maxVisible: 10
    });
    //console.log('html: ' + n.options.id);
}