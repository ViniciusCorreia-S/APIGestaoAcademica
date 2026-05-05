const apiBase = "/api";
let token = localStorage.getItem("token") || "";
let alunos = [];

const elementos = {
    usuario: document.getElementById("usuario"),
    senha: document.getElementById("senha"),
    btnLogin: document.getElementById("btnLogin"),
    loginStatus: document.getElementById("loginStatus"),
    form: document.getElementById("alunoForm"),
    alunoId: document.getElementById("alunoId"),
    nome: document.getElementById("nome"),
    email: document.getElementById("email"),
    matricula: document.getElementById("matricula"),
    dataNascimento: document.getElementById("dataNascimento"),
    cursoId: document.getElementById("cursoId"),
    ativo: document.getElementById("ativo"),
    tabela: document.getElementById("alunosTabela"),
    mensagem: document.getElementById("mensagem"),
    btnAtualizar: document.getElementById("btnAtualizar"),
    btnCancelar: document.getElementById("btnCancelar")
};

function cabecalhosJson() {
    const headers = { "Content-Type": "application/json" };
    if (token) headers.Authorization = `Bearer ${token}`;
    return headers;
}

async function requisicao(url, opcoes = {}) {
    const resposta = await fetch(url, {
        ...opcoes,
        headers: {
            ...cabecalhosJson(),
            ...(opcoes.headers || {})
        }
    });

    const texto = await resposta.text();
    const corpo = texto ? JSON.parse(texto) : {};

    if (!resposta.ok) {
        throw new Error(corpo.mensagem || "Nao foi possivel concluir a operacao.");
    }

    return corpo;
}

function mostrarMensagem(texto, tipo = "success") {
    elementos.mensagem.className = `alert alert-${tipo}`;
    elementos.mensagem.textContent = texto;
}

function atualizarStatusLogin() {
    elementos.loginStatus.className = token ? "alert alert-success mb-0" : "alert alert-secondary mb-0";
    elementos.loginStatus.textContent = token
        ? "Login realizado. As operacoes protegidas podem ser executadas."
        : "Use admin/123456 ou secretaria/123456.";
}

async function login() {
    try {
        const resultado = await requisicao(`${apiBase}/auth/login`, {
            method: "POST",
            body: JSON.stringify({
                usuario: elementos.usuario.value,
                senha: elementos.senha.value
            })
        });

        token = resultado.token;
        localStorage.setItem("token", token);
        atualizarStatusLogin();
    } catch (erro) {
        token = "";
        localStorage.removeItem("token");
        atualizarStatusLogin();
        mostrarMensagem(erro.message, "danger");
    }
}

async function carregarCursos() {
    const resultado = await requisicao(`${apiBase}/cursos`);
    elementos.cursoId.innerHTML = resultado.dados
        .map(curso => `<option value="${curso.id}">${curso.nome}</option>`)
        .join("");
}

async function carregarAlunos() {
    const resultado = await requisicao(`${apiBase}/alunos`);
    alunos = resultado.dados;
    renderizarTabela();
}

function renderizarTabela() {
    if (!alunos.length) {
        elementos.tabela.innerHTML = `<tr><td colspan="5" class="text-center py-4">Nenhum aluno cadastrado.</td></tr>`;
        return;
    }

    elementos.tabela.innerHTML = alunos.map(aluno => `
        <tr>
            <td>${aluno.matricula}</td>
            <td>${aluno.nome}<div class="small text-muted">${aluno.email}</div></td>
            <td>${aluno.cursoNome}</td>
            <td>
                <span class="status-dot ${aluno.ativo ? "active" : "inactive"}"></span>
                ${aluno.ativo ? "Ativo" : "Inativo"}
            </td>
            <td class="text-end">
                <button class="btn btn-outline-secondary btn-sm" onclick="editarAluno(${aluno.id})">Editar</button>
                <button class="btn btn-outline-danger btn-sm" onclick="excluirAluno(${aluno.id})">Excluir</button>
            </td>
        </tr>
    `).join("");
}

function editarAluno(id) {
    const aluno = alunos.find(item => item.id === id);
    if (!aluno) return;

    elementos.alunoId.value = aluno.id;
    elementos.nome.value = aluno.nome;
    elementos.email.value = aluno.email;
    elementos.matricula.value = aluno.matricula;
    elementos.matricula.disabled = true;
    elementos.dataNascimento.value = aluno.dataNascimento;
    elementos.cursoId.value = aluno.cursoId;
    elementos.ativo.checked = aluno.ativo;
}

function limparFormulario() {
    elementos.form.reset();
    elementos.alunoId.value = "";
    elementos.matricula.disabled = false;
    elementos.ativo.checked = true;
}

async function salvarAluno(evento) {
    evento.preventDefault();

    const id = elementos.alunoId.value;
    const corpo = {
        nome: elementos.nome.value,
        email: elementos.email.value,
        dataNascimento: elementos.dataNascimento.value,
        cursoId: Number(elementos.cursoId.value),
        ativo: elementos.ativo.checked
    };

    if (!id) {
        corpo.matricula = elementos.matricula.value;
    }

    try {
        await requisicao(id ? `${apiBase}/alunos/${id}` : `${apiBase}/alunos`, {
            method: id ? "PUT" : "POST",
            body: JSON.stringify(corpo)
        });

        mostrarMensagem(id ? "Aluno atualizado com sucesso." : "Aluno cadastrado com sucesso.");
        limparFormulario();
        await carregarAlunos();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    }
}

async function excluirAluno(id) {
    if (!confirm("Deseja excluir este aluno?")) return;

    try {
        await requisicao(`${apiBase}/alunos/${id}`, { method: "DELETE" });
        mostrarMensagem("Aluno excluido com sucesso.");
        await carregarAlunos();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    }
}

elementos.btnLogin.addEventListener("click", login);
elementos.btnAtualizar.addEventListener("click", carregarAlunos);
elementos.btnCancelar.addEventListener("click", limparFormulario);
elementos.form.addEventListener("submit", salvarAluno);

atualizarStatusLogin();
carregarCursos()
    .then(carregarAlunos)
    .catch(erro => mostrarMensagem(erro.message, "danger"));
