/* File Created: May 23, 2012 */
jQuery(function($) {
    // Num Pad Input
    // ********************
    //wrap the text box you want to have a numpad on in a span with ID =SpanNumericKeyboard
    $('#SpanNumericKeyboard > input').keyboard({
		layout: 'num',
		restrictInput : true, // Prevent keys not in the displayed keyboard from being typed in
		preventPaste : true,  // prevent ctrl-v and right click
		autoAccept : true
});
// Autocomplete demo
var availableTags = ["ActionScript", "AppleScript"];
//var x = document.getElementById("WOHidden");

$('#SpanQwertyKeyboard > input').keyboard({ 
layout: 'qwerty' })
//    .autocomplete({
//        source: availableTags
//                  })
//                  .addAutocomplete()
//                  .addTyping();

});



