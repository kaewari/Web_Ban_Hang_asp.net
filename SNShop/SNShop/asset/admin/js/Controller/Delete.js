function DeleteProduct(Id) {
    if (confirm('Warning!!! Bạn có chắc muốn xóa sản phẩm không') == true) {
        fetch('/Product/Delete_Product', {
            method: 'post',
            body: JSON.stringify({
                'id': Id,
            }),
            headers: {
                'Content-Type': 'application/json'
            }

        }).then(function (res) {
            console.info(res)
            return res.json()
        }).then(function (data) {
            if (data.status == 200) {
                let r = document.getElementById("Product_" + Id)
                r.style.display = 'none'
                console.info(data)
                alert(data.success)
            } else if (data.status == 400) {
                alert(data.error)
            }
        }).catch(err => console.info(err))
    }
}
function DeleteAllProduct() {
    if (confirm('Warning!!! Bạn có chắc muốn xóa tất cả sản phẩm không?') == true) {
        fetch('/Product/Delete_All_Product', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            }

        }).then(function (res) {
            console.info(res)
            return res.json()
        }).then(function (data) {
            if (data.status == 200) {
                console.info(data)
                alert(data.success)
                location.reload(true)
            } else {
                alert(data.error)
            }
        }).catch(err => console.info(err))
    }
}
function DeleteProductImage(Id) {
    if (confirm('Warning!!! Bạn có chắc muốn xóa ảnh không') == true) {
        fetch('/ProductImage/Delete_Product_Image', {
            method: 'post',
            body: JSON.stringify({
                'id': Id,
            }),
            headers: {
                'Content-Type': 'application/json'
            }

        }).then(function (res) {
            console.info(res)
            return res.json()
        }).then(function (data) {
            if (data.status == 200) {
                let r = document.getElementById("Product_Image_" + Id)
                r.style.display = 'none'
                console.info(data)
                alert(data.success)
            } else if (data.status == 400) {
                alert(data.error)
            }
        }).catch(err => console.info(err))
    }
}
function DeleteAllProductImage() {
    if (confirm('Warning!!! Bạn có chắc muốn xóa tất cả ảnh không?') == true) {
        fetch('/ProductImage/Delete_All_Product_Image', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            }

        }).then(function (res) {
            console.info(res)
            return res.json()
        }).then(function (data) {
            if (data.status == 200) {
                console.info(data)
                alert(data.success)
                location.reload(true)
            } else {
                alert(data.error)
            }
        }).catch(err => console.info(err))
    }
}
