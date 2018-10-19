"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/dataHub").build();

connection.on("NewData", function (trackerId, data) {
    //var dataOutputDiv = document.getElementById("dataOutput");
    //dataOutputDiv.innerText = data;

    let gyroX = [];
    let gyroY = [];
    let gyroZ = [];

    let lowAccX = [];
    let lowAccY = [];
    let lowAccZ = [];

    let highAccX = [];
    let highAccY = [];
    let highAccZ = [];

    // go from a two dimensional array to one dimension
    data.forEach((measurement) => {
        gyroX.push(measurement[0]);
        gyroY.push(measurement[1]);
        gyroZ.push(measurement[2]);
        lowAccX.push(measurement[3]);
        lowAccY.push(measurement[4]);
        lowAccZ.push(measurement[5]);
        highAccX.push(measurement[6]);
        highAccY.push(measurement[7]);
        highAccZ.push(measurement[8]);
    });

    // TODO: create a function for this
    lowAccX.forEach((value) => {
        chart.data.datasets[3].data.push(value);
    });
    chart.data.datasets[3].data.splice(0, 10);

    lowAccY.forEach((value) => {
        chart.data.datasets[4].data.push(value);
    });
    chart.data.datasets[4].data.splice(0, 10);

    lowAccZ.forEach((value) => {
        chart.data.datasets[5].data.push(value);
    });
    chart.data.datasets[5].data.splice(0, 10);

    chart.update(0);

    console.log(trackerId);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

// todo: auslagern
let isRecording = false;
document.getElementById("recordingState").addEventListener('click', (e) => {

    if (isRecording) {
        connection.invoke("StopRecording").catch((err) => {
            return console.error(err.toString());
        });
        document.getElementById("recordingState").innerText = "Start Recording";
        isRecording = false;
    } else {
        connection.invoke("StartRecording").catch((err) => {
            return console.error(err.toString());
        });
        document.getElementById("recordingState").innerText = "Stop Recording";
        isRecording = true;
    }
    
    e.preventDefault();
});

connection.invoke('')

Chart.defaults.showLines = true;

const ctx = document.getElementById('myChart').getContext('2d');
let chart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: Array(100).fill(""),
        datasets: [
            {
                label: "gyroX",
                borderColor: 'rgb(255, 99, 132)',
                fill: false,
                lineTension: 0,
                data: Array(100).fill(0)
            },
            {
                label: "gyroY",
                backgroundColor: 'rgba(255, 255, 255, 0)',
                borderColor: 'rgb(255, 153, 0)',
                fill: false,
                lineTension: 0,
                data: Array(100).fill(0)
            },
            {
                label: "gyroZ",
                backgroundColor: 'rgba(255, 255, 255, 0)',
                borderColor: 'rgb(255, 180, 0)',
                lineTension: 0,
                data: Array(100).fill(0)
            },
            {
                label: "lowAccX",
                backgroundColor: 'rgba(255, 255, 255, 0)',
                borderColor: 'rgb(255, 40, 0)',
                lineTension: 0,
                data: Array(100).fill(0)
            },
            {
                label: "lowAccY",
                backgroundColor: 'rgba(255, 255, 255, 0)',
                borderColor: 'rgb(150, 100, 0)',
                data: Array(100).fill(0)
            },
            {
                label: "lowAccZ",
                backgroundColor: 'rgba(255, 255, 255, 0)',
                borderColor: 'rgb(50, 200, 0)',
                data: Array(100).fill(0)
            },
            {
                label: "highAccX",
                backgroundColor: 'rgba(255, 255, 255, 0)',
                borderColor: 'rgb(100, 40, 0)',
                data: Array(100).fill(0)
            },
            {
                label: "highAccY",
                backgroundColor: 'rgba(255, 255, 255, 0)',
                borderColor: 'rgb(50, 153, 0)',
                data: Array(100).fill(0)
            },
            {
                label: "highAccZ",
                backgroundColor: 'rgba(255, 255, 255, 0)',
                borderColor: 'rgb(140, 200, 0)',
                data: Array(100).fill(0)
            }
        ]
    },
    options: {
        scales: {
            yAxes: [{
                ticks: {
                    min: -30000,
                    max: 30000
                }
            }]
        }
    }
});