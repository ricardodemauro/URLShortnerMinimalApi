const app = {
    supaUrl: "https://tqqcjengpfcxqdiptxwv.supabase.co",
    publicId: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InRxcWNqZW5ncGZjeHFkaXB0eHd2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE2NjA0MjQ0NzUsImV4cCI6MTk3NjAwMDQ3NX0.jDbmMJVrE0hWKdR4HaPG67DitSpY8quU5mREMZhlavU",

    init: function () {
        $('[data-toggle="tooltip"]').tooltip()

        app.initBindings();

        // Create a single supabase client for interacting with your database 
        window.supabase = window.supabase.createClient(app.supaUrl, app.publicId)

        app.ui.updateButtons();
    },

    isAuthenticated: () => false,

    initBindings: function () {
        document.getElementById('qsLoginBtn').addEventListener('click', app.events.login);

        document.getElementById('qsLogoutBtn').addEventListener('click', app.events.logout);
    },

    events: {
        login: async function () {
            const oAuthOpts = {
                scopes: 'user:email user:follow'
            }
            const authOptions = {
                provider: 'github'
            }
            const { user, session, error } = await supabase.auth.signIn(authOptions, oAuthOpts)

            const oAuthToken = session.provider_token;
            console.log(oAuthToken)
        },

        logout: async function () {
            const { error } = await supabase.auth.signOut()
            console.log('signed out')
        }
    },

    ui: {
        updateButtons: function () {
            const isAuthenticated = app.isAuthenticated();

            const btnLogout = document.getElementById('qsLogoutBtn');
            btnLogout.disabled = !isAuthenticated;
            btnLogout.style.display = !isAuthenticated ? 'none' : 'inline-block';

            const btnLogin = document.getElementById('qsLoginBtn');
            btnLogin.disabled = isAuthenticated;
            btnLogin.style.display = isAuthenticated ? 'none' : 'inline-block';

            const btnSubmit = document.getElementById('btnSubmit');
            btnSubmit.disabled = !isAuthenticated;

            if (isAuthenticated) {
                $('[data-toggle="tooltip"]').tooltip('disable')
            }
        }
    }
}

window.onload = () => app.init();