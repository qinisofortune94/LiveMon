Highcharts.setOptions({
    global: {
        useUTC: false
    }
});

// Create the chart

$(document).ready(function () {

    Highcharts.stockChart('container0', {
        chart: {
            width: 360,
            events: {
                load: function () {

                    var series = this.series[0];
                    setInterval(function () {
                        // set up the updating of the chart each second
                        $.ajax({
                            type: "POST",
                            url: "KVAGraphDisplay.aspx/Getvalues",
                            data: '{}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            success: SetupDisplay,
                            failure: function (response) {
                                //alert(response.d);
                            },
                            error: function (response) {
                                //alert(response.d);
                            }
                        });

                        function SetupDisplay(response) {
                            var data1 = response.d

                            const data2 = [];
                            const data3 = [];
                            for (var i in data1) {
                                data2.push(data1[i])
                            }
                            for (var i in data2) {
                                data3.push(data2[i][0])
                            }





                            //  var x = String(data2[0][1]); // current time


                            var x = (new Date()).getTime(), // current time
                               y = parseInt(data2[0][0]);
                            //  y = Math.round(Math.random() * 100);
                            series.addPoint([x, y], true, true);
                        }
                    }, 5000);

                }
            }
        },
        credits: { href: 'http://www.livemonitoring.co.za', text: 'Live Monitoring' },
        rangeSelector: {
            buttons: [{
                count: 1,
                type: 'minute',
                text: '1M'
            }, {
                count: 5,
                type: 'minute',
                text: '5M'
            }, {
                type: 'all',
                text: 'All'
            }],
            inputEnabled: false,
            selected: 0
        },

        title: {
            text: 'SIM S.STATION'
        },

        exporting: {
            enabled: false
        },

        series: [{
            name: 'Current Readings',
            data: (function () {
                // generate an array of random data
                var data = [],
                    time = (new Date()).getTime(),
                    i;

                for (i = -999; i <= 0; i += 1) {
                    data.push([
                        time + i * 1000,
                        0
                    ]);
                }
                return data;
            }()),

            tooltip: {
                valueSuffix: ' KVA'
            },


        }]
    });
});
    $(document).ready(function () {
    Highcharts.stockChart('container1', {
        chart: {
            width: 360,
            events: {
                load: function () {

                    var series = this.series[0];
                    setInterval(function () {
                        // set up the updating of the chart each second
                        $.ajax({
                            type: "POST",
                            url: "KVAGraphDisplay.aspx/Getvalues",
                            data: '{}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            success: SetupDisplay,
                            failure: function (response) {
                                //alert(response.d);
                            },
                            error: function (response) {
                                //alert(response.d);
                            }
                        });

                        function SetupDisplay(response) {
                            var data1 = response.d;
                           
                            const data2 = [];
                            const data3 = [];
                            for (var i in data1) {
                                data2.push(data1[i])
                            }
                            for (var i in data2) {
                                data3.push(data2[i][0])
                            }





                            //  var x = String(data2[0][1]); // current time


                            var x = (new Date()).getTime(), // current time
                               y = parseInt(data2[1][0]);
                            //  y = Math.round(Math.random() * 100);
                            series.addPoint([x, y], true, true);
                        }
                    }, 5000);

                }
            }
        },
        credits: { href: 'http://www.livemonitoring.co.za', text: 'Live Monitoring' },
        rangeSelector: {
            buttons: [{
                count: 1,
                type: 'minute',
                text: '1M'
            }, {
                count: 5,
                type: 'minute',
                text: '5M'
            }, {
                type: 'all',
                text: 'All'
            }],
            inputEnabled: false,
            selected: 0
        },

        title: {
            text: 'SECTION 17 SS'
        },

        exporting: {
            enabled: false
        },

        series: [{
            name: 'Current Readings',
            data: (function () {
                // generate an array of random data
                var data = [],
                    time = (new Date()).getTime(),
                    i;

                for (i = -999; i <= 0; i += 1) {
                    data.push([
                        time + i * 1000,
                        0
                    ]);
                }
                return data;
            }()),

            tooltip: {
                valueSuffix: ' KVA'
            },


        }]
    });
    });

    $(document).ready(function () {
        Highcharts.stockChart('container2', {
            chart: {
                width : 360,
                events: {
                    load: function () {

                        var series = this.series[0];
                        setInterval(function () {
                            // set up the updating of the chart each second
                            $.ajax({
                                type: "POST",
                                url: "KVAGraphDisplay.aspx/Getvalues",
                                data: '{}',
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",

                                success: SetupDisplay,
                                failure: function (response) {
                                    //alert(response.d);
                                },
                                error: function (response) {
                                    //alert(response.d);
                                }
                            });

                            function SetupDisplay(response) {
                                var data1 = response.d
                           
                                const data2 = [];
                                const data3 = [];
                                for (var i in data1) {
                                    data2.push(data1[i])
                                }
                                for (var i in data2) {
                                    data3.push(data2[i][0])
                                }





                                //  var x = String(data2[0][1]); // current time


                                var x = (new Date()).getTime(), // current time
                                   y = parseInt(data2[2][0]);
                                //  y = Math.round(Math.random() * 100);
                                series.addPoint([x, y], true, true);
                            }
                        }, 5000);

                    }
                }
           
    },
            credits: { href: 'http://www.livemonitoring.co.za', text: 'Live Monitoring' },
        rangeSelector: {
            buttons: [{
                count: 1,
                type: 'minute',
                text: '1M'
            }, {
                count: 5,
                type: 'minute',
                text: '5M'
            }, {
                type: 'all',
                text: 'All'
            }],
            inputEnabled: false,
            selected: 0
        },

        title: {
            text: 'NGO S.STATION'
        },

        exporting: {
            enabled: false
        },

        series: [{
            name: 'Current Readings',
            data: (function () {
                // generate an array of random data
                var data = [],
                    time = (new Date()).getTime(),
                    i;

                for (i = -999; i <= 0; i += 1) {
                    data.push([
                        time + i * 1000,
                        0
                    ]);
                }
                return data;
            }()),

            tooltip: {
                valueSuffix: ' KVA'
            },


        }]
    });

    });