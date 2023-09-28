<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KVAGraphDisplay1.aspx.cs" Inherits="website2016V2.KVAGraphDisplay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" runat="server">
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/highcharts-more.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/modules/export-data.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <%-- <script src="http://code.jquery.com/jquery-migrate-1.1.0.js"></script>--%>


    <h3>KVA Guage Display </h3>
    <div class="col-md">
    </div>
    <div id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default" style="width: 100%">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a id="first" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne"><strong></strong>
                    </a>
                </h4>
            </div>

            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" style="padding: 1%">
                <br />
                <br />
                <div id="container"></div>
                <label id="label"></label>
            </div>
            <br />
        </div>
    </div>

    <script >

        $(function () { Draw1stChart(); });

        
        const randomInt = function (multiplier) {
            return Math.round(Math.random() * multiplier);
        };

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

            yAxis: {
                min: 0,
                max: 4000,
                minorTickInterval: 'auto',
                plotBands: [{
                    from: 0,
                    to: 2000,
                    color: '#55BF3B' // green
                }, {
                    from: 2000,
                    to: 3200,
                    color: '#DDDF0D' // yellow
                }, {
                    from: 3200,
                    to: 4000,
                    color: '#DF5353' // red
                }],
            },
            series: [{
                name: 'Maxium Demand Reached',
                data: [0],
                tooltip: {
                    valueSuffix: ' KVA'
                },

            },
                        {
                            name: 'Current Reading',
                            data: [0],
                            tooltip: {
                                valueSuffix: ' KVA'
                            }
                        }],
        };

      

        function Draw1stChart() {
            //getValues;
            //debugger;
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

                success: SetupDisplay ,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
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
                div.text = data2[i][2];
                div.ClientIDMode = "Static";
                document.getElementById('container').appendChild(div);
                // Create chart
                charts[i] = Highcharts.chart(GaugeID, options[i]);
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
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
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
        }

    </script>
</asp:Content>
