let email = document.getElementById('email').textContent;
let finalCost = document.getElementById('final-cost').textContent;
let table = document.querySelector('table > tbody');

function finalize() {
    if (table.children.length == 0)
       return alert('Няма избрана храна. Моля избере първо храна.')

    fetch('/cart/finalize', {
        method: 'POST',
        headers: {
            'Content-Type': 'text/plain'
        },
        body: email + ', ' + finalCost
    })
        .then(async res => {
            let html = await res.text();
            document.querySelector('html').innerHTML = html;
        })
}