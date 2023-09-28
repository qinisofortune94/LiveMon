//const randomInt = function (multiplier) {
//    return Math.round(Math.random() * multiplier);
//};

// $(function () { Draw1stChart(); });


////const randomInt = function (multiplier) {
////    return Math.round(Math.random() * multiplier);
//};

$(document).ready(function () {
    Draw1stChart();
});

const option = {
    chart: {
        type: 'gauge'
    },
    title: {
        text: ''
    },
    pane: {
        startAngle: -150,
        endAngle: 150
    },
    credits: { href: 'http://www.livemonitoring.co.za', text: 'Live Monitoring' },
    yAxis: {
        min: 0,
        max: 20000,

        minorTickInterval: 'auto',



        plotBands: [{
            from: 0,
            to: 10000,
            color: '#55BF3B' // green
        }, {
            from: 10000,
            to: 16000,
            color: '#DDDF0D' // yellow
        }, {
            from: 16000,
            to: 20000,
            color: '#DF5353' // red
        }],


    },
    series: [
                {
                    name: 'Current Reading',
                    data: [0],
                    tooltip: {
                        valueSuffix: ' KVA'
                    }
                },
                {
                    name: 'Maxium Demand Reached',
                    data: [0],
                    tooltip: {
                        valueSuffix: ' KVA'
                    },
                    dial: {
                        backgroundColor: 'red'
                    }

                },


    ],
};



function Draw1stChart() {
    //getValues;
    
    getSensors();

};


function getSensors() {
    //   setInterval(function () {
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

};

function SetupDisplay(response) {
    var data1 = response.d
    //debugger;
    const data2 = [];
    const data = [];
    for (var i in data1) {
        data2.push(data1[i])
    }
    for (var i in data2) {
        data.push(data2[i][1])
    }
    const options = [];
    const charts = [];
    const containerWidth = document.getElementById('container').offsetWidth;
    var count = data.length - 1;
    for (let i = 0; i < data.length; ++i) {
        // Set data in options
        option.series[0].data[0] = parseInt(data2[i][0]);
        option.series[1].data[0] = parseInt(data2[i][1]);
        options[i] = JSON.parse(JSON.stringify(option));
        // options[i] = update(JSON.parse(JSON.stringify(option)));
        // Create div for chart
       
        const div = document.createElement("div");
        var GaugeID = 'gauge-' + i
        div.id = GaugeID;
        div.style.display = 'inline-block';
        div.style.width = containerWidth / data.length + 'px';
        var station = "";
        station = String(data2[i][2]);
        var input = document.createElement('input');
        input.type = "text";
        input.setAttribute("value", station);
        input.setAttribute("style", "font-weight:bold");
        input.setAttribute("style", "font-size:large");

        
        div.text = data2[i][2];
        div.ClientIDMode = "Static";
        
        document.getElementById('container'+i).prepend(div);
        
        // Create chart
        charts[i] = Highcharts.chart(GaugeID, options[i]);
        
       // var graph = getElementById('container' + i);
      //  graph.style.width = containerWidth / data.length + 'px';
     //   document.getElementById('gauge-' + i).appendChild(graph);
      //  document.getElementById('gauge-' + i).appendChild(input);
       
       
        
    }
    setInterval(function () { getValues() }, 5000);

};

function getValues() {
    //   setInterval(function () {
    $.ajax({
        type: "POST",
        url: "KVAGraphDisplay.aspx/Getvalues",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: OnSuccess,
        failure: function (response) {
            //alert(response.d);
        },
        error: function (response) {
            //alert(response.d);
        }
    });

};

function OnSuccess(response) {
    //debugger;
    var data1 = response.d
    //debugger;
    const data2 = [];
    const data = [];
    for (var i in data1) {
        data2.push(data1[i])
    }
    for (var i in data2) {
        data.push(data2[i][1])
    }

    const options = [];
    const charts = [];
    const containerWidth = document.getElementById('container').offsetWidth;
    var count = data.length - 1;

    for (let i = 0; i < data.length; ++i) {

        var GaugeID = 'gauge-' + i
        var index = $("#" + GaugeID).data('highchartsChart');
        var chart = Highcharts.charts[index];
        // var chart = $("#" + GaugeID).highcharts();
        //debugger;
        chart.series[0].data[0].update(parseInt(data2[i][0]));
        chart.series[1].data[0].update(parseInt(data2[i][1]));
        chart.redraw();
    }
    getDate();

}

function getDate() {
    var dt = new Date().toLocaleString();
    document.getElementById('mylabel').innerHTML = dt;
    
}