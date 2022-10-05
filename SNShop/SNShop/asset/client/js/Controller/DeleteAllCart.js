function DeleteAll() {
    if (confirm('Bạn có chắc muốn xóa tất cả sản phẩm không?') == true) {
        fetch('/Cart/DeleteAllCart', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            }

        }).then(function (res) {
            console.info(res)
            return res.json()
        }).then(function (data) {
            if (data.status == 200) {
                let countCart = document.getElementById('countCart')
                let countTotal = document.getElementById('countTotal')
                let dd1 = document.getElementById('dd1')
                let dd2 = document.getElementById('dd2')
                countCart.innerText = data.cartsData.total_quantity
                countTotal.innerText = FormatCurrency(data.cartsData.total_amount)
                dd1.innerText = data.cartsData.total_quantity
                dd2.innerText = data.cartsData.total_quantity
                let r = document.getElementById("cart");
                r.style.display = 'none'
                console.info(data)
            } else
                alert(data.error)
        }).catch(err => console.info(err))
    }
}