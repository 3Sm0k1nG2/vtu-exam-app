const renderElements = {
    th: (data) => `<th>${data}</th>`,
    td: (data) => `<td>${data}</td>`,
    tr: (data) => `<tr>${data}</tr>`,
    actionButtons: (renderUpdate = true) => `${renderUpdate ? '<button class="record-action btn btn-warning" style="max-width: 5rem; width: 5rem">Update</button>' : ''}<button class="record-action btn btn-danger" style="max-width: 5rem; width: 5rem">Delete</button>`
}

const isValid = {
    email: (email) => email.includes('@'),
    id: (id) => {
        if (id === null || id === '')
            return false;

        id = Number(id);

        if (isNaN(id))
            return false;

        if (!Number.isInteger(id))
            return false;

        return true;
    }
}

const confirmation = {
    deleteBy: {
        Id: (id) => confirm(`Do you really want to delete User with ID ${id}?`),
        Email: (email) => confirm(`Do you really want to delete User with Email ${email}?`)
    }
}

const dbsActions = {
    users: {
        getAll: function () {
            return fetch('/admin/GetUsers')
                .then(res => res.json())
                .then(data => data)
                .catch(err => null);
        },
        getOneByEmail: () => {
            let email = prompt('Users: Enter email');

            if (!isValid.email(email))
                return;

            return fetch(`/admin/GetUser`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: email
            })
                .then(res => res.json())
                .then(data => [data])
                .catch(err => null);
        },
        getOneById: (id) => {
            if (!id) {
                id = prompt('Users: Enter id');
            }

            if (!isValid.id(id))
                return;

            return fetch(`/admin/GetUser`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: id
            })
                .then(res => res.json())
                .then(data => [data])
                .catch(err => null);
        },
        deleteByEmail: () => {
            let email = prompt('Users: Enter email');

            if (!isValid.email(email))
                return false;

            if (!confirmation.deleteBy.Email(email))
                return false;

            return fetch(`/admin/DeleteUser`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: email
            })
                .then(result => renderAllRecords())
                .catch(err => null);
        },
        deleteById: (id) => {
            if (!id) {
                id = prompt('Users: Enter id');
            }

            if (!isValid.id(id))
                return false;

            if (!confirmation.deleteBy.Id(id))
                return false;

            return fetch(`/admin/DeleteUser`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: id
            })
                .then(result => renderAllRecords())
                .catch(err => null);
        }
    },
    dishes: {
        getAll: () => {
            return fetch('/admin/GetDishes')
                .then(res => res.json())
                .then(data => data)
                .catch(err => null);
        },
        getOne: (id) => {
            if (!id) {
                id = prompt('Dishes: Enter id');
            }

            if (!isValid.id(id))
                return;

            return fetch(`/admin/GetDish`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: id
            })
                .then(res => res.json())
                .then(data => [data])
                .catch(err => null);

        },
        create: () => {
            return window.location.pathname = '/dish/create';
        },
        update: (id) => {
            if (!id) {
                id = prompt('Dishes: Enter id');
            }

            if (isValid.id(id))
                return window.location.pathname = '/dish/update/' + id;
        },
        delete: (id) => {
            if (!id) {
                id = prompt('Dishes: Enter id');
            }

            if (!isValid.id(id))
                return false;

            if (!confirm(`Do you really want to delete Dish with ID ${id}?`))
                return false;

            return fetch(`/admin/DeleteDish`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: id
            })
                .then(() => renderAllRecords())
                .catch(err => null);
        },

    },
    orders: {
        getAll: () => {
            return fetch('/admin/GetOrdersDetailed')
                .then(res => res.json())
                .then(data => data)
                .catch(err => null);
        },
        getAllByUserId: (id) => {
            if (!id) {
                id = prompt('Orders: Enter user id');
            }

            if (!isValid.id(id))
                return;

            return fetch('/admin/GetOrdersDetailed', {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: id
            })
                .then(res => res.json())
                .then(data => data)
                .catch(err => null);
        },
        getAllByUserEmail: () => {
            let email = prompt('Orders: Enter user email');

            if (!isValid.email(email))
                return;

            return fetch('/admin/GetOrdersDetailed', {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: email
            })
                .then(res => res.json())
                .then(data => data)
                .catch(err => null);
        },
        getOne: (id) => {
            if (!id) {
                id = prompt('Orders: Enter id');
            }

            if (!isValid.id(id))
                return;

            return fetch('/admin/GetOrderDetailed', {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: id
            })
                .then(res => res.json())
                .then(data => [data])
                .catch(err => null)
        },
        deleteById: (id) => {
            if (!id)
                id = Number(prompt('Orders: Enter id'));

            if (!isValid.id(id))
                return false;

            if (!confirmation.deleteBy.Id(id))
                return false;

            return fetch(`/admin/DeleteOrder`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain'
                },
                body: id
            })
                .then(() => renderAllRecords(false))
                .catch(err => null);
        },
    }
}

let actions;
const resultElement = document.getElementById('result');
const toolsHTMLElement = document.getElementById('tools');

toolsHTMLElement.addEventListener('click', (e) => {
    if (e.target.tagName !== 'BUTTON')
        return;

    clearResults();

    actions = dbsActions[e.target.parentElement.parentElement.id];

    let action = actions[e.target.dataset.action]();

    if (action == null)
        return;

    action.then(data => {
        if (data == null)
            return;

        let [keys, records] = processData(data);
        renderResults(keys, records, e.target.parentElement.parentElement.id !== 'orders');
    })
    .catch(err => err)
})

resultElement.addEventListener('click', async (e) => {
    if (e.target.tagName !== 'BUTTON')
        return;

    if (e.target.classList[0] === 'record-action') {
        let id = Number(e.target.parentElement.parentElement.firstChild.textContent);

        if (e.target.textContent === 'Update') {
            let cmd = actions.update || actions.updateById;
            cmd(id);
        }
        else if (e.target.textContent === 'Delete') {
            let cmd = actions.delete || actions.deleteById;
            cmd(id);
        }
    }
})

function renderAllRecords(renderUpdate) {
    actions.getAll()
        .then(data => {
            let [keys, values] = processData(data);
            renderResults(keys, values, renderUpdate);
        })
}

function renderResults(keys, data, renderUpdate) {
    keys = keys.map(x => x[0].toUpperCase() + x.slice(1,))
    keys.push('Actions');

    data.push();

    resultElement.innerHTML = `
        <table class="table">
            <thead class="sticky bg-light">
                <tr> ${keys.map(x => renderElements.th(x)).join('')}</tr>
            </thead>
            <tbody>
                ${data.map(x => {
        xValues = Object.values(x);
        xValues.push(renderElements.actionButtons(renderUpdate));

        return renderElements.tr(xValues.map(y => renderElements.td(y === null ? '' : y)).join(''))
    }).join('')
        }
            </tbody>
        </table >
        `;
}

function processData(data) {
    let keys = Object.keys(data[0]);
    let fractionIndex = keys.findIndex(x => x === 'price')
    let records = [];
    data.forEach(x => {
        let values = Object.values(x);
        records.push(values.map((x, i) => {
            if (x === null || x === undefined)
                return '';

            if (i === fractionIndex)
                return x.toFixed(2);

            return x;
        }));
    })

    return [keys, records];
}

function clearResults() {
    resultElement.innerHTML = '';
}


