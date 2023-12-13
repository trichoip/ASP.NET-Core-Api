
function handleCredentialResponse(response) {
    console.log(response)
    console.log("Encoded JWT ID token: " + response.credential);
    console.log(jwtDecode(response.credential))

    const responsePayload = decodeJwtResponse(response.credential);
    console.log("ID: " + responsePayload.sub);
    console.log('Full Name: ' + responsePayload.name);
    console.log('Given Name: ' + responsePayload.given_name);
    console.log('Family Name: ' + responsePayload.family_name);
    console.log("Image URL: " + responsePayload.picture);
    console.log("Email: " + responsePayload.email);

    fetch('/api/test/get', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            token: response.credential,
            data: responsePayload,
        }),
    }).then(response => {
        return response.json();
    }).then(data => {
        console.log(data);
    }).catch(error => {
        console.error('There was a problem with the fetch operation:', error);
    });
}

//window.onload = function () {
//    google.accounts.id.initialize({
//        client_id: "748375698529-ul7shovile6hhu77ucj6snib9gqec3hc.apps.googleusercontent.com",
//        callback: handleCredentialResponse
//    });
//    google.accounts.id.renderButton(
//        document.getElementById("buttonDiv"),
//        { theme: "outline", size: "large" }  // customization attributes
//    );
//    google.accounts.id.prompt(); // also display the One Tap dialog
//}

function decodeJwtResponse(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

const button = document.getElementById('signout_button');
button.onclick = () => {
    console.log('Signed out');
    google.accounts.id.disableAutoSelect();
}

