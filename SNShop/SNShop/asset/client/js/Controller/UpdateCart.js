function updateCart(id, obj) {
    Update(id, obj);
}

function Update(id, obj) {
    fetch('/Cart/UpdateCart', {
        method: 'post',
        body: JSON.stringify({
            'id': id,
            'quantity': parseFloat(obj.value)
        }),
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (res) {
        console.info(res)
        return res.json()
    }).then(function (data) {
        let countCart = document.getElementById('countCart')
        let countTotal = document.getElementById('countTotal')
        let countTotal1 = document.getElementById(id)
        let dd1 = document.getElementById('dd1')
        let dd2 = document.getElementById('dd2')
        countCart.innerText = data.cartsData.total_quantity
        countTotal.innerText = FormatCurrency(data.cartsData.total_amount)
        countTotal1.innerText = FormatCurrency(data.cartItem.amount)
        dd1.innerText = data.cartsData.total_quantity
        dd2.innerText = data.cartsData.total_quantity
        
        console.info(data)
    }).catch(function (err) {
        console.info(err)
    })
}