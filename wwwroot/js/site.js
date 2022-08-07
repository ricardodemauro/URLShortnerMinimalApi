document.getElementById('btnSubmit')
    .addEventListener('click', e => {
        e.preventDefault();
        handleSubmitAsync();
    });

document.getElementById('url')
    .addEventListener('keyup', function (evt) {
        if (evt.code === 'Enter') {
            event.preventDefault();
            handleSubmitAsync();
        }
    });

async function handleSubmitAsync() {
    const url = document.getElementById('url').value;

    const json = { 'url': url };

    const accessTokenScopes = {
        scope: 'create:url',
    };
    const accessToken = await auth0.getTokenSilently(accessTokenScopes);

    const headers = { 'content-type': 'application/json', 'Authorization': `Bearer ${accessToken}` };

    fetch('/urls', { method: 'post', body: JSON.stringify(json), headers: headers })
        .then(apiResult => {
            return new Promise(resolve => apiResult.json()
                .then(json => resolve({ ok: apiResult.ok, status: apiResult.status, json: json }))
            );
        })
        .then(({ json, ok, status }) => {
            if (ok) {
                const anchor = `<a href=${json.shortUrl} target="_blank">${json.shortUrl}</a>`;
                document.getElementById('urlResult').innerHTML = anchor;
            }
            else {
                alert(json.errorMessage);
            }
        });
}

const fnLoadRecords = async () => {
    $.addTemplateFormatter({
        shortUrlFormatter: function (value, template) {
            const full = location.protocol + '//' + location.host;
            return `${full}/${value}`;
        },
        dateFormatter: function (value, template) {
            const d = new Date(value);
            const options = {
                year: 'numeric',
                month: 'short',
                day: 'numeric',
                hour: 'numeric',
                minute: 'numeric'
            };
            return d.toLocaleString('en-us', options)
        }
    });

    const apiResult = await fetch('/urls');
    if (apiResult.ok) {
        const json = await apiResult.json();

        console.table(json)
        $("#table-results > tbody").loadTemplate($("#template-table"), json, { error: function (e) { console.log(e); } });
    }
};

window.cred = {
    "domain": "rmaurodev.us.auth0.com",
    "clientId": "BEUw3cx6VYyMft8m6OvzZlXTl0nC7ErE",
    "audience": "https://heroku-url-short.herokuapp.com/"
};

window.auth0 = null;

const configureClient = async () => {
    auth0 = await createAuth0Client({
        domain: window.cred.domain,
        client_id: window.cred.clientId,
        audience: window.cred.audience   // NEW - add the audience value
    });
};

const updateUI = async () => {
    const isAuthenticated = await auth0.isAuthenticated();

    const btnLogout = document.getElementById('qsLogoutBtn');
    btnLogout.disabled = !isAuthenticated;
    btnLogout.style.display = !isAuthenticated ? 'none' : 'inline-block';

    const btnLogin = document.getElementById('qsLoginBtn');
    btnLogin.disabled = isAuthenticated;
    btnLogin.style.display = isAuthenticated ? 'none' : 'inline-block';
};

const login = async () => {
    await auth0.loginWithRedirect({
        'redirect_uri': window.location.origin,
        'scope': "openid profile email create:url"
    });
};

const logout = () => {
    auth0.logout();
}

window.onload = async () => {
    await fnLoadRecords();

    await configureClient();
    updateUI();
    const isAuthenticated = await auth0.isAuthenticated();

    if (isAuthenticated) {
        // show the gated content
        return;
    }
    // NEW - check for the code and state parameters
    const query = window.location.search;
    if (query.includes("code=") && query.includes("state=")) {

        // Process the login state
        await auth0.handleRedirectCallback();

        updateUI();

        // Use replaceState to redirect the user away and remove the querystring parameters
        window.history.replaceState({}, document.title, "/");
    }
}