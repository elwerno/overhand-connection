# overhand-connection

Connects the trackers in the local network to a local C# server and shows the data of the tracker in a live Graph.

![demo](https://user-images.githubusercontent.com/16801528/47205976-14bb5800-d3ba-11e8-90c3-887fc63be4ec.jpg)

With the click of the button it allows to record data from the trackers into text files. 
This makes it a lot easier to gather new data for the machine learning algorithm to process.

# Run it locally

## Prerequisities

- Overhand Router (with Password)
- Computer
    - dotnet core installed
- Trackers

## Instructions

1 Connect your computer to the Overhand Network (via WiFi or Ethernet Cable)
2 Set your IP Address to `11.0.0.2`
3 Start the server (`dotnet run --project TcpBrokerTest.csproj`)
4 Go to `http://localhost:yourPortNumber/index.html`
5 Enjoy!

# Possible Improvements 

- In the recording feature if you stop a recording the last line of data will most likely be cut off. This should be improved.
- Show the other data of the trackers in the graph aswell
- Rename the project
