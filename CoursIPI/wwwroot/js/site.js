﻿const uri = 'api/Books';
let books = [];
let _token;

document.getElementById('logoutButton').addEventListener('click', function () {
    localStorage.removeItem('jwtToken');
    window.location.href = 'login.html';
});


function getItems(token) {
    _token = token;
    fetch(uri, {
        headers: {
            'Authorization': `Bearer ${token}` 
        }
    }).then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem(token) {
    const addNameTextbox = document.getElementById('add-name');
    const addAuthorTextbox = document.getElementById('add-author');
    const addEditionTextbox = document.getElementById('add-edition');
    const addDescriptionTextbox = document.getElementById('add-description');
    const addEANTextbox = document.getElementById('add-ean');

    const item = {
        name: addNameTextbox.value.trim(),
        author: addAuthorTextbox.value.trim(),
        edition: addEditionTextbox.value.trim(),
        description: addDescriptionTextbox.value.trim(),
        ean: addEANTextbox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json', 
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems(token);
            addNameTextbox.value = '';
            addAuthorTextbox.value = '';
            addEditionTextbox.value = '';
            addDescriptionTextbox.value = '';
            addEANTextbox.value = '';

        })
        .catch(error => console.error('Unable to add item.', error));
    hideForm();
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${_token}` 
        }
    })
        .then(() => getItems(_token))
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
    document.getElementById('editForm').style.display = 'block';
    const item = books.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-author').value = item.author;
    document.getElementById('edit-edition').value = item.edition;
    document.getElementById('edit-description').value = item.description;
    document.getElementById('edit-ean').value = item.ean;
    document.getElementById('edit-id').value = item.id;
}

function updateItem(token) {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        name: document.getElementById('edit-name').value.trim(),
        author: document.getElementById('edit-author').value.trim(),
        edition: document.getElementById('edit-edition').value.trim(),
        description: document.getElementById('edit-description').value.trim(),
        ean: document.getElementById('edit-ean').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}` 
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems(token))
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'book in your book list' : 'books in your book list';

    document.getElementById('counter').innerText = `You have ${itemCount} ${name}`;
}

function _displayItems(data) {
    const container = document.getElementById('books');
    container.innerHTML = '';

    _displayCount(data.length);

    data.forEach(item => {
        let bookDiv = document.createElement('div');
        bookDiv.className = 'book'; 

        let titleElement = document.createElement('h2');
        titleElement.innerText = item.name;

        let authorElement = document.createElement('p');
        authorElement.innerText = `Author: ${item.author}`;

        let editionElement = document.createElement('p');
        editionElement.innerText = `Edition: ${item.edition}`;

        let descriptionElement = document.createElement('p');
        descriptionElement.innerText = `Description: ${item.description}`;

        let eanElement = document.createElement('p');
        eanElement.innerText = `EAN: ${item.ean}`;

        let editButton = document.createElement('button');
        editButton.innerText = 'Edit';
        editButton.className = 'edit-button'; 
        editButton.addEventListener('click', () => {
            displayEditForm(item.id);
        });

        let deleteButton = document.createElement('button');
        deleteButton.innerText = 'Delete';
        deleteButton.className = 'delete-button';
        deleteButton.addEventListener('click', () => {
            deleteItem(item.id);
        });

        bookDiv.appendChild(titleElement);
        bookDiv.appendChild(authorElement);
        bookDiv.appendChild(editionElement);
        bookDiv.appendChild(descriptionElement);
        bookDiv.appendChild(eanElement);
        bookDiv.appendChild(editButton);
        bookDiv.appendChild(deleteButton);
        bookDiv.appendChild(deleteButton);

        container.appendChild(bookDiv);
    });

    books = data;
}

function showForm() {
    const addForm = document.getElementById('addForm');
    addForm.style.display = 'block';
    window.scrollTo({
        top: document.body.scrollHeight,
        behavior: 'smooth' 
    });
}

function hideForm() {
    const addForm = document.getElementById('addForm');
    addForm.style.display = 'none';
}

const showFormButton = document.getElementById('showFormButton');
showFormButton.addEventListener('click', showForm);