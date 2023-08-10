// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.addEventListener('load', function () {
    let historyStack = JSON.parse(sessionStorage.getItem('historyStack')) || [];
    if (!historyStack.includes(location.href)) {
        historyStack.push(location.href);
    }
    sessionStorage.setItem('historyStack', JSON.stringify(historyStack));
    history.pushState(null, null, location.href);

    let specificUrl = "https://localhost:7213";
    if (window.location.href === specificUrl && !sessionStorage.getItem('justLoggedOut')) {
        console.log("The specific URL was found in the history stack.");
        fetch('/Login/SignOut', { method: 'GET' })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.text();
            })
            .then(data => {
                sessionStorage.clear();
                sessionStorage.setItem('justLoggedOut', true);

                location.reload();

            })
            .catch(error => {
                console.error('There has been a problem with your fetch operation:', error);
            });
    } else {
        sessionStorage.removeItem('justLoggedOut');
    }
});
