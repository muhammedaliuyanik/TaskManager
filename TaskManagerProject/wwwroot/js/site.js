document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem('token');
    const profileLink = document.getElementById('profileLink');
    const loginLink = document.getElementById('loginLink');
    const registerLink = document.getElementById('registerLink');
    const logoutLink = document.getElementById('logoutLink');

    if (token) {
        profileLink.style.display = 'block';
        logoutLink.style.display = 'block';
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
});

function logout() {
    localStorage.removeItem('token');
    alert('Logout successful');
    window.location.href = '/Home/Login';
}
