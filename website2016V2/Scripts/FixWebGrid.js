/* File Created: July 10, 2012 */

function igtbl_dispose(obj) {
    if (ig_csom.IsNetscape || ig_csom.IsNetscape6)
        return;
    for (var item in obj) {
        for (var item in obj) {
            if (typeof (obj[item]) != "undefined" && obj[item] != null && !obj[item].tagName && !obj[item].disposing && typeof (obj[item]) != "string" && obj.hasOwnProperty(item)) {
                try {

                    obj[item].disposing = true;
                    igtbl_dispose(obj[item]);
                } catch (exc1) { ; }
            }
            try {
                delete obj[item];
            } catch (exc2) {
                return;
            }
        }
    } 
}