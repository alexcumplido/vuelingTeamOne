import { Login } from "./Login.jsx"
import { Form } from "./Form.jsx"

export function Administrative (){
    return (
        <section className="component-admin">
            <Form/>
            <Login/>
        </section>
    );
}