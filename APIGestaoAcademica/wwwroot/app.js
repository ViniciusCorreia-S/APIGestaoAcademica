const apiBase = "/api";
let token = localStorage.getItem("token") || "";
let alunos = [];
let alunoIdParaExcluir = null;

const elementos = {
    usuario: document.getElementById("usuario"),
    senha: document.getElementById("senha"),
    btnLogin: document.getElementById("btnLogin"),
    btnLogout: document.getElementById("btnLogout"),
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
    btnCancelar: document.getElementById("btnCancelar"),
    buscaAluno: document.getElementById("buscaAluno"),
    filtroCurso: document.getElementById("filtroCurso"),
    filtroStatus: document.getElementById("filtroStatus"),
    modalExclusao: document.getElementById("modalExclusao"),
    alunoExclusaoNome: document.getElementById("alunoExclusaoNome"),
    btnConfirmarExclusao: document.getElementById("btnConfirmarExclusao")
};

const modalExclusao = new bootstrap.Modal(elementos.modalExclusao);

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
        throw new Error(corpo.mensagem || "Não foi possível concluir a operação.");
    }

    return corpo;
}

function escaparHtml(valor) {
    return String(valor ?? "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}

function normalizarTexto(valor) {
    return String(valor ?? "").toLowerCase().trim();
}

function definirCarregando(botao, carregando, textoCarregando = "Carregando...") {
    if (!botao.dataset.textoOriginal) {
        botao.dataset.textoOriginal = botao.textContent;
    }

    botao.disabled = carregando;
    botao.textContent = carregando ? textoCarregando : botao.dataset.textoOriginal;
}

function mostrarMensagem(texto, tipo = "success") {
    elementos.mensagem.className = `alert alert-${tipo}`;
    elementos.mensagem.textContent = texto;
}

function atualizarStatusLogin() {
    const logado = Boolean(token);

    elementos.loginStatus.className = logado ? "alert alert-success mb-0" : "alert alert-secondary mb-0";
    elementos.loginStatus.textContent = logado
        ? "Login realizado. As operações protegidas podem ser executadas."
        : "Use admin/123456 ou secretaria/123456.";

    elementos.btnLogout.classList.toggle("d-none", !logado);
}

async function login() {
    definirCarregando(elementos.btnLogin, true, "Entrando...");

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
        mostrarMensagem("Login realizado com sucesso.");
    } catch (erro) {
        token = "";
        localStorage.removeItem("token");
        atualizarStatusLogin();
        mostrarMensagem(erro.message, "danger");
    } finally {
        definirCarregando(elementos.btnLogin, false);
    }
}

function logout() {
    token = "";
    localStorage.removeItem("token");
    atualizarStatusLogin();
    mostrarMensagem("Sessão encerrada.");
}

async function carregarCursos() {
    const resultado = await requisicao(`${apiBase}/cursos`);
    const opcoes = resultado.dados
        .map(curso => `<option value="${curso.id}">${escaparHtml(curso.nome)}</option>`)
        .join("");

    elementos.cursoId.innerHTML = opcoes;
    elementos.filtroCurso.innerHTML = `<option value="">Todos os cursos</option>${opcoes}`;
}

async function carregarAlunos() {
    definirCarregando(elementos.btnAtualizar, true, "Atualizando...");

    try {
        const resultado = await requisicao(`${apiBase}/alunos`);
        alunos = resultado.dados;
        renderizarTabela();
    } finally {
        definirCarregando(elementos.btnAtualizar, false);
    }
}

function filtrarAlunos() {
    const busca = normalizarTexto(elementos.buscaAluno.value);
    const cursoId = elementos.filtroCurso.value;
    const status = elementos.filtroStatus.value;

    return alunos.filter(aluno => {
        const textoAluno = normalizarTexto(`${aluno.nome} ${aluno.email} ${aluno.matricula}`);
        const correspondeBusca = !busca || textoAluno.includes(busca);
        const correspondeCurso = !cursoId || String(aluno.cursoId) === cursoId;
        const correspondeStatus = !status || (status === "ativo" ? aluno.ativo : !aluno.ativo);

        return correspondeBusca && correspondeCurso && correspondeStatus;
    });
}

function renderizarTabela() {
    const alunosFiltrados = filtrarAlunos();

    if (!alunosFiltrados.length) {
        elementos.tabela.innerHTML = `<tr><td colspan="5" class="text-center py-4">Nenhum aluno encontrado.</td></tr>`;
        return;
    }

    elementos.tabela.innerHTML = alunosFiltrados.map(aluno => `
        <tr>
            <td>${escaparHtml(aluno.matricula)}</td>
            <td>${escaparHtml(aluno.nome)}<div class="small text-muted">${escaparHtml(aluno.email)}</div></td>
            <td>${escaparHtml(aluno.cursoNome)}</td>
            <td>
                <span class="status-dot ${aluno.ativo ? "active" : "inactive"}"></span>
                ${aluno.ativo ? "Ativo" : "Inativo"}
            </td>
            <td class="text-end">
                <button class="btn btn-outline-secondary btn-sm" onclick="editarAluno(${aluno.id})">Editar</button>
                <button class="btn btn-outline-danger btn-sm" onclick="abrirModalExclusao(${aluno.id})">Excluir</button>
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

    const botaoSalvar = elementos.form.querySelector("button[type='submit']");
    definirCarregando(botaoSalvar, true, "Salvando...");

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
    } finally {
        definirCarregando(botaoSalvar, false);
    }
}

function abrirModalExclusao(id) {
    const aluno = alunos.find(item => item.id === id);
    if (!aluno) return;

    alunoIdParaExcluir = id;
    elementos.alunoExclusaoNome.textContent = `${aluno.nome} - matrícula ${aluno.matricula}`;
    modalExclusao.show();
}

async function confirmarExclusao() {
    if (!alunoIdParaExcluir) return;

    definirCarregando(elementos.btnConfirmarExclusao, true, "Excluindo...");

    try {
        await requisicao(`${apiBase}/alunos/${alunoIdParaExcluir}`, { method: "DELETE" });
        mostrarMensagem("Aluno excluído com sucesso.");
        modalExclusao.hide();
        alunoIdParaExcluir = null;
        await carregarAlunos();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    } finally {
        definirCarregando(elementos.btnConfirmarExclusao, false);
    }
}

elementos.btnLogin.addEventListener("click", login);
elementos.btnLogout.addEventListener("click", logout);
elementos.btnAtualizar.addEventListener("click", carregarAlunos);
elementos.btnCancelar.addEventListener("click", limparFormulario);
elementos.form.addEventListener("submit", salvarAluno);
elementos.buscaAluno.addEventListener("input", renderizarTabela);
elementos.filtroCurso.addEventListener("change", renderizarTabela);
elementos.filtroStatus.addEventListener("change", renderizarTabela);
elementos.btnConfirmarExclusao.addEventListener("click", confirmarExclusao);

atualizarStatusLogin();
carregarCursos()
    .then(carregarAlunos)
    .catch(erro => mostrarMensagem(erro.message, "danger"));
