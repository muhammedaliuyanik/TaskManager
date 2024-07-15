document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem('token');
    const profileLink = document.getElementById('profileLink');
    const loginLink = document.getElementById('loginLink');
    const registerLink = document.getElementById('registerLink');
    const logoutLink = document.getElementById('logoutLink');
    const projectsLink = document.getElementById('projectsLink');
    const tasksLink = document.getElementById('tasksLink');

    if (token) {
        profileLink.style.display = 'block';
        logoutLink.style.display = 'block';
        projectsLink.style.display = 'block';
        tasksLink.style.display = 'block';
        loginLink.style.display = 'none';
        registerLink.style.display = 'none';
    }

    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const data = {
                username: document.getElementById('username').value,
                password: document.getElementById('password').value
            };
            try {
                const response = await fetch('/api/user/login', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data)
                });
                const result = await response.json();
                if (response.ok) {
                    localStorage.setItem('token', result.token);
                    alert('Login successful');
                    window.location.href = '/Home/Profile';
                } else {
                    alert(result.message);
                }
            } catch (error) {
                console.error('Error:', error);
            }
        });
    }

    async function fetchWithAuth(url, options = {}) {
        const token = localStorage.getItem('token');
        const headers = options.headers || {};
        headers['Authorization'] = `Bearer ${token}`;
        options.headers = headers;
        const response = await fetch(url, options);
        return response;
    }

    // Example usage
    if (projectsLink) {
        projectsLink.addEventListener('click', async () => {
            const response = await fetchWithAuth('/Project');
            if (response.ok) {
                window.location.href = '/Project';
            } else {
                alert('Unauthorized');
            }
        });
    }

    if (tasksLink) {
        tasksLink.addEventListener('click', async () => {
            const response = await fetchWithAuth('/Task');
            if (response.ok) {
                window.location.href = '/Task';
            } else {
                alert('Unauthorized');
            }
        });
    }
});

function logout() {
    localStorage.removeItem('token');
    alert('Logout successful');
    window.location.href = '/Home/Login';
}
