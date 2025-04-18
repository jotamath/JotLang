import React, { useState, useEffect } from 'react';

function App() {
    const [message, setMessage] = useState('');
    const [user, setUser] = useState(null);

    useEffect(() => {
        // Busca mensagem da API
        fetch('/api/hello')
            .then(res => res.json())
            .then(data => setMessage(data.message));

        // Busca dados do usuário
        fetch('/api/user')
            .then(res => res.json())
            .then(data => setUser(data));
    }, []);

    return (
        <div className="App">
            <header>
                <h1>JotLang + React</h1>
            </header>
            
            <main>
                <section>
                    <h2>Mensagem da API</h2>
                    <p>{message}</p>
                </section>

                {user && (
                    <section>
                        <h2>Dados do Usuário</h2>
                        <p>ID: {user.id}</p>
                        <p>Nome: {user.name}</p>
                        <p>Email: {user.email}</p>
                    </section>
                )}
            </main>
        </div>
    );
}

export default App; 