 // await hubContext.Clients.All.SendAsync("LoadProducts", _context.GetAll().ToList());
var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
connection.start();

connection.on("LoadProducts", function (data) {
    var tr = '';
    console.log(data);
    data.forEach(function (item) {
        tr += '<tr>';
        tr += '<td>' + item.flowerBouquetName + '</td>';
        tr += '<td>' + item.description + '</td>';
        tr += '<td>' + item.unitPrice + '</td>';
        tr += '<td>' + item.unitsInStock + '</td>';
        tr += '<td>' + item.flowerBouquetStatus + '</td>';
        tr += '</tr>';
        console.log(item);

    })
    $('#tableBody').html(tr);
})

