// nếu trên url không có tham số thì giá trị trả về là null
const urlParams = new URLSearchParams(window.location.search);

const SearchString1 = urlParams.get('SearchString'); // trả về là null nếu không có tham số query string trên url
const SearchField1 = urlParams.get('SearchField');// trả về là null nếu không có tham số query string trên url
const pageIndex1 = urlParams.get('pageIndex');// trả về là null nếu không có tham số query string trên url

console.log(SearchString1);
console.log(SearchField1);
console.log(pageIndex1);

console.log("==============================================");

// nếu trên url không có tham số thì giá trị trả về là null
const params = new Proxy(new URLSearchParams(window.location.search), {
    get: (searchParams, prop) => searchParams.get(prop),
});

let SearchString2 = params.SearchString;// trả về là null nếu không có tham số query string trên url
let SearchField2 = params.SearchField;// trả về là null nếu không có tham số query string trên url
let pageIndex2 = params.pageIndex;// trả về là null nếu không có tham số query string trên url

console.log(SearchString2);
console.log(SearchField2);
console.log(pageIndex2);

console.log("==============================================");

const PageIndex = document.getElementById('PageIndex').value;
const SearchString = document.getElementById('SearchString').value; // giá trị mặc định là empty
const SearchField = document.getElementById('SearchField').value;

console.log(PageIndex);
console.log(SearchString);
console.log(SearchField);

//===================================================================================================

var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
connection.start().then(function () {
    fetch('/api/Flower/CountProduct')
        .then(response => response.json())
        .then(data => {
            console.log(data);
            $('#TotalProduct').html('S:Total Product: ' + data);
        });
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("Messages", function (data) {
    console.log(data);
    $('#messages').html(data);
})

connection.on("TotalProducts", function () {
    fetch('/api/Flower/CountProduct')
        .then(response => response.json())
        .then(data => {
            console.log(data);
            $('#TotalProduct').html('S:Total Product: ' + data);
        }).catch(function (err) {
            console.error(err.toString());
        })
})

// lưu ý : tham số kiểu số như int, float ,... truyền vào phải khác null nếu truyền param vào mà param null
// thì nó sẽ giống https://localhost:7155/api/Flower/GetAllProduct?SearchField=null&SearchString=null&pageIndex=null
// thì vào server api sẽ bị lỗi 400 , errors pageIndex "The value 'null' is not valid.", vì pageIndex là kiểu số nên không thể truyền null vào được, còn string thì được truyền vào là null
connection.on("GetProducts", function () {
    fetch(`/api/Flower/GetAllProduct?SearchField=${SearchField}&SearchString=${SearchString}&pageIndex=${PageIndex}`)
        .then(response => response.json())
        .then(data => {
            var tr = data.map(item => `
                                         <tr>
                                            <td>${item.flowerBouquetName}</td>
                                            <td>${item.description}</td>
                                            <td>${item.unitPrice}</td>
                                            <td>${item.unitsInStock}</td>
                                            <td>${item.flowerBouquetStatus}</td>
                                            <td>${item.category.categoryName}</td>
                                            <td>${item.supplier.supplierName}</td>
                                            <td>
                                                <a href="/Flower/Edit?id=${item.flowerBouquetId}">Edit</a> |
                                                <a href="/Flower/Details?id=${item.flowerBouquetId}">Details</a> |
                                                <a href="/Flower/Delete?id=${item.flowerBouquetId}">Delete</a>
                                            </td>
                                        </tr>
                                        `).join('');

            $('#dataProduct').html(tr);

            console.log(data);
            console.log(tr);
        }).catch(function (err) {
            console.error(err.toString());
        })
})