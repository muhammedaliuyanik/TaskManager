document.addEventListener('DOMContentLoaded', function () {
    const registerForm = document.getElementById('registerForm');
    const loginForm = document.getElementById('loginForm');
    const profileForm = document.getElementById('profileForm');

    if (registerForm) {
        registerForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const data = {
                username: document.getElementById('username').value,
                password: document.getElementById('password').value,
                email: document.getElementById('email').value,
                firstName: document.getElementById('firstName').value,
                lastName: document.getElementById('lastName').value
            };
            try {
                const response = await fetch('/api/user/register', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data)
                });
                const result = await response.json();
                console.log(result);
                if (response.ok) {
                    alert('Registration successful!');
                    window.location.href = '/Home/Login';
                } else {
                    alert('Registration failed: ' + result.message);
                }
            } catch (error) {
                console.error('Error:', error);
                alert('Registration failed: ' + error.message);
            }
        });
    }

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

                let result;
                try {
                    result = await response.json();
                } catch (error) {
                    throw new Error('Response is not valid JSON: ' + await response.text());
                }

                if (response.ok) {
                    localStorage.setItem('token', result.token);
                    alert('Login successful!');
                    window.location.href = '/Home/Profile';
                } else {
                    alert('Login failed: ' + result.message);
                }
            } catch (error) {
                console.error('Error:', error);
                alert('Login failed: ' + error.message);
            }
        });
    }

    if (profileForm) {
        profileForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const data = {
                username: document.getElementById('username').value,
                email: document.getElementById('email').value,
                firstName: document.getElementById('firstName').value,
                lastName: document.getElementById('lastName').value
            };
            const token = localStorage.getItem('token');
            try {
                const response = await fetch('/api/user/updateProfile', {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    },
                    body: JSON.stringify(data)
                });

                let result;
                try {
                    result = await response.json();
                } catch (error) {
                    throw new Error('Response is not valid JSON: ' + await response.text());
                }

                if (response.ok) {
                    alert('Profile updated successfully!');
                } else {
                    alert('Profile update failed: ' + result.message);
                }
            } catch (error) {
                console.error('Error:', error);
                alert('Profile update failed: ' + error.message);
            }
        });

        const fetchProfile = async () => {
            const token = localStorage.getItem('token');
            try {
                const response = await fetch('/api/user/profile', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });
                const profile = await response.json();
                document.getElementById('username').value = profile.username;
                document.getElementById('email').value = profile.email;
                document.getElementById('firstName').value = profile.firstName;
                document.getElementById('lastName').value = profile.lastName;
            } catch (error) {
                console.error('Error:', error);
            }
        };

        fetchProfile();
    }
});
