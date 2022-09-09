function addCart(id, name, price) {
    Add(id, name, price);
}

function Add(id, name, price) {
    fetch('/Cart/AddCart', {
        method: 'post',
        body: JSON.stringify({
            'id': id,
            'name': name,
            'price': parseFloat(price),
        }),
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (res) {
        console.info(res)
        return res.json()
    }).then(function (data) {
        let dd1 = document.getElementById('dd1')
        let dd2 = document.getElementById('dd2')
        dd1.innerText = data.cartsData.total_quantity
        dd2.innerText = data.cartsData.total_quantity
        alert("Thêm vào giỏ hàng thành công.");
        console.info(data)
    }).catch(function (err) {
        console.info(err)
    })
}