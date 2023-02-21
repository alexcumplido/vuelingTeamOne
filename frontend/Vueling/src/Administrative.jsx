import { Form } from "./Form.jsx"
import { useState } from "react";

export function Administrative (){
    const [auth, setAut] = useState(true);
    const [emailLogin, setEmailLogin] = useState('');
    const [passwordLogin, setPasswordLogin] = useState('');
    const handleEmailLogin = (e) => setEmailLogin(e.target.value);
    const handlePasswordLogin = (e) => setPasswordLogin(e.target.value);
    
    const [formData, setFormData] = useState({
        name: '',
        username: '',
        email: '',
        password: '',
        role: '',
    });

    const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFormData((prevFormData) => ({
        ...prevFormData,
        [name]: value,
    }));
    };

    async function loginUser(){
        try{
            const response = await fetch('fetchData', {
                method: 'put',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify({
                "emailLogin": emailLogin,
                "login": passwordLogin,
            })
            })
            const responseAuth = await response.json();
            if(responseAuth.message = 'suscces') {
                setAut(true);
                window.localStorage.setItem("currentUser", JSON.stringify({
                "emailLogin": emailLogin,
                "login": passwordLogin,
            }));
            }
        } catch(err){
            console.log(err)
        }
    } 
    async function signupUser(){
        try {
            const response = await fetch('/urlTofETCH', {
            method: 'POST',
            body: JSON.stringify(formData),
            headers: {
            'Content-Type': 'application/json',
            },
            });
            const responseAuth = await response.json();
                if(responseAuth.message = 'suscces') {
                    setAut(true);
                } else {
                    alert('Not registered')
                }
        } catch (error) {
            console.log(error)
        }
    }

    const handleSubmitSignup = async (event) => {
        event.preventDefault();
        signupUser();
    };

    const handleSubmitLogin = (event) => {
        event.preventDefault();
        loginUser();  
    };

    return (
        <section className="component-admin">
            {
                auth ? 
                <>
                 <Form/> 
                <button
                    className="control-select"
                    onClick={() => {
                    window.localStorage.removeItem("currentUser");
                    setAut(false);
                 }}
                >
                    Logout
                </button>
                </>
               : 
                <p className="control-label">Dear user, please login to select params</p>
            }
            {
                !auth && 
                <section className="forms-login-register">
                    <form className="form-login" onSubmit={handleSubmitLogin}>
                        <p className="control-label">Login</p>
                    <div>
                        <label className="control-label" htmlFor="email">Email:</label>
                        <input
                        className="control-select"
                        type="email"
                        id="email"
                        value={emailLogin}
                        onChange={handleEmailLogin}
                        required
                        />
                    </div>
                    <div>
                        <label className="control-label" htmlFor="password">Password:</label>
                        <input
                        className="control-select"
                        type="password"
                        id="password"
                        value={passwordLogin}
                        onChange={handlePasswordLogin}
                        required
                        />
                    </div>
                    <button type="submit" className="control-select">Log In</button>
                </form>
                    <form className="form-register" onSubmit={handleSubmitSignup}>
                        <p  className="control-label">Register</p>
                        <label className="control-label">
                            Name:
                            <input className="control-select" type="text" name="name" value={formData.name} onChange={handleInputChange} />
                        </label>
                        <label className="control-label">
                            Username:
                            <input className="control-select" type="text" name="username" value={formData.username} onChange={handleInputChange} />
                        </label>
                        <label className="control-label">
                            Email:
                            <input className="control-select" type="email" name="email" value={formData.email} onChange={handleInputChange} />
                        </label>
                        <label className="control-label">
                            Password:
                            <input className="control-select" type="password" name="password" value={formData.password} onChange={handleInputChange} />
                        </label>
                        <label className="control-label">
                            Role:
                            <input className="control-select" type="text" name="role" value={formData.role} onChange={handleInputChange}/>
                        </label>
                        <button type="submit" className="control-select">Register</button>
                    </form>
                </section>
            }
        </section>
    );
}

