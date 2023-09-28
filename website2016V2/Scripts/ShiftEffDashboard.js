Highcharts.getOptions().colors = Highcharts.map(Highcharts.getOptions().colors, function (color) {
    //return {
    //    radialGradient: {
    //        cx: 0.5,
    //        cy: 0.3,
    //        r: 0.7
    //    },
    //    stops: [
    //        [0, color],
    //        [1, Highcharts.Color(color).brighten(-0.3).get('rgb')] // darken
    //    ]
    //};
});

function DrawChart() {
   

    //$('.filters-btn').on('click', function () {
    //    $('.filters-panel').toggle('show');
    //    if ($.cookie('showhide') == 'hide') {
    //        $.cookie('showhide', 'show');
    //    }
    //    else {
    //        $.cookie('showhide', 'hide');
    //    };
    //});
    //if ($.cookie('showhide') == 'hide') {
    //    $('.filters-hide').hide();
    //};
   
    for (i = 0; i < 350; i++) {
        try {
            var mydiv = document.getElementById('container' + i.toString());

            if (mydiv != null) {
                var myhide = document.getElementById('MyArray' + i.toString());
                //alert('Found it ' + myhide.value);
                //String arrValue = "\"" + TotalMade.ToString("#.00") + "\","; 0
                //arrValue += "\"" + TotalPlannedMade.ToString("#.00") + "\","; 1
                //arrValue += "\"" + TotalDownTime.ToString("#.00") + "\","; 2
                //arrValue += "\"" + TotalGreenTime.ToString("#.00") + "\","; 3
                //arrValue += "\"" + TotalRedTime.ToString("#.00") + "\","; 4
                //arrValue += "\"" + TotalUnallocated.ToString("#.00") + "\","; 5
                //arrValue += "\"" + TotalMinutesTime.ToString("#.00") + "\""; 6
                //machname 7
                //machid 8
                //operator 9
                //Shift start end 10
                //Login time start 11
                //logoff if logged off 12
                var array = myhide.value.split('|');
                //debugger;
               // if (charts['container' + i.toString()] == null)
                //{
                //    charts['container' + i.toString()] = Highcharts.chart('container' + i.toString(), {
                //        chart: {
                //            type: 'bar'
                //        },
                //        title: {
                //            text: array[9] + ' Activity' + ' ' + array[10]
                //        },
                //        xAxis: {
                //            categories: [array[7]]
                //        },
                //        yAxis: {
                //            min: 0,
                //            title: {
                //                text: 'Total Time'
                //            }
                //        },
                //        legend: {
                //            reversed: true
                //        },
                //        plotOptions: {
                //            series: {
                //                stacking: 'normal'
                //            }
                //        },
                //        series: [{
                //            name: 'UnAllocatedTime',
                //            data: [parseFloat(array[5])]
                //        }, {
                //            name: 'GreenTime',
                //            data: [parseFloat(array[3])]
                //        }, {
                //            name: 'RedTime',
                //            data: [parseFloat(array[4])]
                //        }, {
                //            name: 'DownTime',
                //            data: [parseFloat(array[2])]
                //        }]
                //    });
                //}
               // else
                {//update data
                    //charts['container' + i.toString()].series[0].setData([parseFloat(array[5])], true);
                    //charts['container' + i.toString()].series[1].setData([parseFloat(array[3])], true);
                    //charts['container' + i.toString()].series[2].setData([parseFloat(array[4])], true);
                    //charts['container' + i.toString()].series[3].setData([parseFloat(array[2])], true);
                    //charts['container' + i.toString()].highcharts().redraw();
                }
                
                //remove so we can add it again
                //myhide.parentNode.removeChild(myhide);
            };

        } catch (e) {

        };
    };
    
};

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

//global charts
var charts = {};

$(document).ready(function () {
    // Build the chart
    
   // DrawChart();
   // $('.filters-hide').hide();
   // $.cookie('showhide', 'hide');
    
});