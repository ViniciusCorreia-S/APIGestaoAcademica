// =========Variáveis globais
const apiBase = "/api";
let token = localStorage.getItem("token") || "";
let alunos = [];
let cursos = [];
let disciplinas = [];
let matriculas = [];
let exclusaoAtual = null;

// =============Elementos DOM
const elementos = {
    loginPage: document.getElementById("loginPage"),
    homePage: document.getElementById("homePage"),
    loginForm: document.getElementById("loginForm"),
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
    dashTotalAlunos: document.getElementById("dashTotalAlunos"),
    dashAlunosAtivos: document.getElementById("dashAlunosAtivos"),
    dashTotalCursos: document.getElementById("dashTotalCursos"),
    dashMediaAlunosCurso: document.getElementById("dashMediaAlunosCurso"),
    dashTotalDisciplinas: document.getElementById("dashTotalDisciplinas"),
    dashCargaHoraria: document.getElementById("dashCargaHoraria"),
    dashTotalMatriculas: document.getElementById("dashTotalMatriculas"),
    dashUltimaMatricula: document.getElementById("dashUltimaMatricula"),
    dashAlunosPorCurso: document.getElementById("dashAlunosPorCurso"),
    dashResumoRelatorios: document.getElementById("dashResumoRelatorios"),
    dashUltimasMatriculas: document.getElementById("dashUltimasMatriculas"),
    cursoForm: document.getElementById("cursoForm"),
    cursoCadastroId: document.getElementById("cursoCadastroId"),
    cursoNome: document.getElementById("cursoNome"),
    cursoCodigo: document.getElementById("cursoCodigo"),
    cursoDuracaoSemestres: document.getElementById("cursoDuracaoSemestres"),
    cursosTabela: document.getElementById("cursosTabela"),
    btnCancelarCurso: document.getElementById("btnCancelarCurso"),
    disciplinaForm: document.getElementById("disciplinaForm"),
    disciplinaId: document.getElementById("disciplinaId"),
    disciplinaNome: document.getElementById("disciplinaNome"),
    disciplinaCodigo: document.getElementById("disciplinaCodigo"),
    disciplinaCargaHoraria: document.getElementById("disciplinaCargaHoraria"),
    disciplinaCursoId: document.getElementById("disciplinaCursoId"),
    disciplinasTabela: document.getElementById("disciplinasTabela"),
    btnCancelarDisciplina: document.getElementById("btnCancelarDisciplina"),
    matriculaForm: document.getElementById("matriculaForm"),
    matriculaAlunoId: document.getElementById("matriculaAlunoId"),
    matriculaDisciplinaId: document.getElementById("matriculaDisciplinaId"),
    matriculasTabela: document.getElementById("matriculasTabela"),
    modalExclusao: document.getElementById("modalExclusao"),
    exclusaoTexto: document.getElementById("exclusaoTexto"),
    registroExclusaoNome: document.getElementById("registroExclusaoNome"),
    btnConfirmarExclusao: document.getElementById("btnConfirmarExclusao"),
    modalLoginErro: document.getElementById("modalLoginErro"),
    loginErroMensagem: document.getElementById("loginErroMensagem")
};

// =============Inicialização dos modais
const modalExclusao = new bootstrap.Modal(elementos.modalExclusao);
const modalLoginErro = new bootstrap.Modal(elementos.modalLoginErro);

function cabecalhosJson() {
    const headers = { "Content-Type": "application/json" };
    if (token) headers.Authorization = `Bearer ${token}`;
    return headers;
}

// ===================Função genérica para fazer requisições à API
async function requisicao(url, opcoes = {}) {
    const resposta = await fetch(url, {
        ...opcoes,
        headers: {
            ...cabecalhosJson(),
            ...(opcoes.headers || {})
        }
    });

    const texto = await resposta.text();
    let corpo = {};

    try {
        corpo = texto ? JSON.parse(texto) : {};
    } catch {
        const mensagem = texto.includes("MySql") || texto.includes("Exception")
            ? "Nao foi possivel acessar os dados. Verifique se o banco de dados esta configurado e disponivel."
            : texto.slice(0, 220) || "Resposta invalida da API.";

        corpo = { mensagem };
    }

    if (!resposta.ok) {
        throw new Error(corpo.mensagem || "Não foi possível concluir a operação.");
    }

    return corpo;
}

// ============Substitui caracteres especiais por suas entidades HTML para evitar XSS
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
    const configuracoes = {
        success: { titulo: "Sucesso", icone: "OK" },
        danger: { titulo: "Atenção", icone: "!" },
        warning: { titulo: "Aviso", icone: "!" },
        info: { titulo: "Informação", icone: "i" }
    };
    const config = configuracoes[tipo] || configuracoes.info;

    elementos.mensagem.className = `app-alert app-alert-${tipo}`;
    elementos.mensagem.setAttribute("role", "alert");
    elementos.mensagem.innerHTML = `
        <div class="app-alert-icon" aria-hidden="true"><span>${config.icone}</span></div>
        <div class="app-alert-content">
            <strong>${config.titulo}</strong>
            <span>${escaparHtml(texto)}</span>
        </div>
        <button class="app-alert-close" type="button" aria-label="Fechar alerta">X</button>
    `;

    elementos.mensagem.querySelector(".app-alert-close").addEventListener("click", esconderMensagem);
}

function esconderMensagem() {
    elementos.mensagem.className = "app-alert d-none";
    elementos.mensagem.removeAttribute("role");
    elementos.mensagem.textContent = "";
}

function mostrarTelaLogin() {
    elementos.loginPage.classList.remove("d-none");
    elementos.homePage.classList.add("d-none");
    elementos.loginStatus.className = "alert alert-secondary mt-4 mb-0";
    elementos.loginStatus.textContent = "Use admin/123456 ou secretaria/123456.";
}

function mostrarHome() {
    elementos.loginPage.classList.add("d-none");
    elementos.homePage.classList.remove("d-none");
}

async function iniciarHome() {
    mostrarHome();
    esconderMensagem();
    renderizarDashboard();

    try {
        await carregarDadosHome();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    }
}

async function carregarDadosHome() {
    await carregarCursos();
    await carregarAlunos();
    await carregarDisciplinas();
    await carregarMatriculas();
    renderizarDashboard();
}

// ====================Função de login
async function login(evento) {
    evento.preventDefault();
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
        await iniciarHome();
    } catch (erro) {
        token = "";
        localStorage.removeItem("token");
        elementos.loginErroMensagem.textContent = erro.message || "Usuário ou senha inválidos.";
        modalLoginErro.show();
    } finally {
        definirCarregando(elementos.btnLogin, false);
    }
}

function logout() {
    token = "";
    localStorage.removeItem("token");
    esconderMensagem();
    mostrarTelaLogin();
}

async function carregarCursos() {
    const resultado = await requisicao(`${apiBase}/cursos`);
    cursos = resultado.dados;
    preencherSelectsCursos();
    renderizarTabelaCursos();
}

function preencherSelectsCursos() {
    const opcaoInicial = `<option value="" disabled selected>Selecione um curso</option>`;
    const opcoesCursos = cursos
        .map(curso => `<option value="${curso.id}">${escaparHtml(curso.nome)}</option>`)
        .join("");

    elementos.cursoId.innerHTML = opcaoInicial + opcoesCursos;
    elementos.disciplinaCursoId.innerHTML = opcaoInicial + opcoesCursos;
    elementos.filtroCurso.innerHTML = `<option value="">Todos os cursos</option>${opcoesCursos}`;
}

function renderizarTabelaCursos() {
    if (!cursos.length) {
        elementos.cursosTabela.innerHTML = `<tr><td colspan="4" class="text-center py-4">Nenhum curso cadastrado.</td></tr>`;
        return;
    }

    elementos.cursosTabela.innerHTML = cursos.map(curso => `
        <tr>
            <td>${escaparHtml(curso.codigo)}</td>
            <td>${escaparHtml(curso.nome)}</td>
            <td>${curso.duracaoSemestres} semestres</td>
            <td class="text-end">
                <button class="btn btn-outline-secondary btn-sm" onclick="editarCurso(${curso.id})">Editar</button>
                <button class="btn btn-outline-danger btn-sm" onclick="abrirExclusaoCurso(${curso.id})">Excluir</button>
            </td>
        </tr>
    `).join("");
}

function editarCurso(id) {
    const curso = cursos.find(item => item.id === id);
    if (!curso) return;

    elementos.cursoCadastroId.value = curso.id;
    elementos.cursoNome.value = curso.nome;
    elementos.cursoCodigo.value = curso.codigo;
    elementos.cursoDuracaoSemestres.value = curso.duracaoSemestres;
}

function limparFormularioCurso() {
    elementos.cursoForm.reset();
    elementos.cursoCadastroId.value = "";
}

async function salvarCurso(evento) {
    evento.preventDefault();

    const id = elementos.cursoCadastroId.value;
    const corpo = {
        nome: elementos.cursoNome.value,
        codigo: elementos.cursoCodigo.value,
        duracaoSemestres: Number(elementos.cursoDuracaoSemestres.value)
    };

    const botaoSalvar = elementos.cursoForm.querySelector("button[type='submit']");
    definirCarregando(botaoSalvar, true, "Salvando...");

    try {
        await requisicao(id ? `${apiBase}/cursos/${id}` : `${apiBase}/cursos`, {
            method: id ? "PUT" : "POST",
            body: JSON.stringify(corpo)
        });

        mostrarMensagem(id ? "Curso atualizado com sucesso." : "Curso cadastrado com sucesso.");
        limparFormularioCurso();
        await carregarCursos();
        await carregarAlunos();
        await carregarDisciplinas();
        renderizarDashboard();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    } finally {
        definirCarregando(botaoSalvar, false);
    }
}

async function carregarAlunos() {
    const resultado = await requisicao(`${apiBase}/alunos`);
    alunos = resultado.dados;
    renderizarTabelaAlunos();
    preencherSelectAlunosMatricula();
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

function renderizarTabelaAlunos() {
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
                <button class="btn btn-outline-danger btn-sm" onclick="abrirExclusaoAluno(${aluno.id})">Excluir</button>
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
        renderizarDashboard();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    } finally {
        definirCarregando(botaoSalvar, false);
    }
}

async function carregarDisciplinas() {
    const resultado = await requisicao(`${apiBase}/disciplinas`);
    disciplinas = resultado.dados;
    renderizarTabelaDisciplinas();
    preencherSelectDisciplinas();
}

function renderizarTabelaDisciplinas() {
    if (!disciplinas.length) {
        elementos.disciplinasTabela.innerHTML = `<tr><td colspan="5" class="text-center py-4">Nenhuma disciplina cadastrada.</td></tr>`;
        return;
    }

    elementos.disciplinasTabela.innerHTML = disciplinas.map(disciplina => `
        <tr>
            <td>${escaparHtml(disciplina.codigo)}</td>
            <td>${escaparHtml(disciplina.nome)}</td>
            <td>${escaparHtml(disciplina.cursoNome)}</td>
            <td>${disciplina.cargaHoraria}h</td>
            <td class="text-end">
                <button class="btn btn-outline-secondary btn-sm" onclick="editarDisciplina(${disciplina.id})">Editar</button>
                <button class="btn btn-outline-danger btn-sm" onclick="abrirExclusaoDisciplina(${disciplina.id})">Excluir</button>
            </td>
        </tr>
    `).join("");
}

function editarDisciplina(id) {
    const disciplina = disciplinas.find(item => item.id === id);
    if (!disciplina) return;

    elementos.disciplinaId.value = disciplina.id;
    elementos.disciplinaNome.value = disciplina.nome;
    elementos.disciplinaCodigo.value = disciplina.codigo;
    elementos.disciplinaCargaHoraria.value = disciplina.cargaHoraria;
    elementos.disciplinaCursoId.value = disciplina.cursoId;
}

function limparFormularioDisciplina() {
    elementos.disciplinaForm.reset();
    elementos.disciplinaId.value = "";
}

async function salvarDisciplina(evento) {
    evento.preventDefault();

    const id = elementos.disciplinaId.value;
    const corpo = {
        nome: elementos.disciplinaNome.value,
        codigo: elementos.disciplinaCodigo.value,
        cargaHoraria: Number(elementos.disciplinaCargaHoraria.value),
        cursoId: Number(elementos.disciplinaCursoId.value)
    };

    const botaoSalvar = elementos.disciplinaForm.querySelector("button[type='submit']");
    definirCarregando(botaoSalvar, true, "Salvando...");

    try {
        await requisicao(id ? `${apiBase}/disciplinas/${id}` : `${apiBase}/disciplinas`, {
            method: id ? "PUT" : "POST",
            body: JSON.stringify(corpo)
        });

        mostrarMensagem(id ? "Disciplina atualizada com sucesso." : "Disciplina cadastrada com sucesso.");
        limparFormularioDisciplina();
        await carregarDisciplinas();
        renderizarDashboard();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    } finally {
        definirCarregando(botaoSalvar, false);
    }
}

function preencherSelectAlunosMatricula() {
    const alunosAtivos = alunos.filter(aluno => aluno.ativo);
    const opcaoInicial = `<option value="" disabled selected>Selecione um aluno</option>`;

    if (!alunosAtivos.length) {
        elementos.matriculaAlunoId.innerHTML = `${opcaoInicial}<option value="" disabled>Nenhum aluno ativo disponível</option>`;
        return;
    }

    elementos.matriculaAlunoId.innerHTML = opcaoInicial + alunosAtivos
        .map(aluno => `<option value="${aluno.id}">${escaparHtml(aluno.nome)} - ${escaparHtml(aluno.matricula)}</option>`)
        .join("");
}

function preencherSelectDisciplinas() {
    const opcaoInicial = `<option value="" disabled selected>Selecione uma disciplina</option>`;

    if (!disciplinas.length) {
        elementos.matriculaDisciplinaId.innerHTML = `${opcaoInicial}<option value="" disabled>Nenhuma disciplina disponível</option>`;
        return;
    }

    elementos.matriculaDisciplinaId.innerHTML = opcaoInicial + disciplinas
        .map(disciplina => `<option value="${disciplina.id}">${escaparHtml(disciplina.codigo)} - ${escaparHtml(disciplina.nome)}</option>`)
        .join("");
}

async function carregarMatriculas() {
    const resultado = await requisicao(`${apiBase}/matriculas`);
    matriculas = resultado.dados;
    renderizarTabelaMatriculas();
}

function renderizarTabelaMatriculas() {
    if (!matriculas.length) {
        elementos.matriculasTabela.innerHTML = `<tr><td colspan="5" class="text-center py-4">Nenhuma matrícula realizada.</td></tr>`;
        return;
    }

    elementos.matriculasTabela.innerHTML = matriculas.map(matricula => `
        <tr>
            <td>${escaparHtml(matricula.alunoNome)}</td>
            <td>${escaparHtml(matricula.disciplinaNome)}</td>
            <td>${formatarData(matricula.dataMatricula)}</td>
            <td>${escaparHtml(matricula.status)}</td>
            <td class="text-end">
                <button class="btn btn-outline-danger btn-sm" onclick="abrirExclusaoMatricula(${matricula.id})">Excluir</button>
            </td>
        </tr>
    `).join("");
}

function formatarData(valor) {
    if (!valor) return "";
    return new Date(valor).toLocaleDateString("pt-BR");
}

function calcularPercentual(valor, total) {
    if (!total) return 0;
    return Math.round((valor / total) * 100);
}

function renderizarDashboard() {
    const alunosAtivos = alunos.filter(aluno => aluno.ativo).length;
    const alunosInativos = alunos.length - alunosAtivos;
    const cargaHorariaTotal = disciplinas.reduce((total, disciplina) => total + Number(disciplina.cargaHoraria || 0), 0);
    const mediaAlunosCurso = cursos.length ? (alunos.length / cursos.length).toFixed(1).replace(".", ",") : "0";
    const ultimaMatricula = matriculas
        .slice()
        .sort((a, b) => new Date(b.dataMatricula) - new Date(a.dataMatricula))[0];

    elementos.dashTotalAlunos.textContent = alunos.length;
    elementos.dashAlunosAtivos.textContent = `${alunosAtivos} ativos e ${alunosInativos} inativos`;
    elementos.dashTotalCursos.textContent = cursos.length;
    elementos.dashMediaAlunosCurso.textContent = `${mediaAlunosCurso} alunos por curso`;
    elementos.dashTotalDisciplinas.textContent = disciplinas.length;
    elementos.dashCargaHoraria.textContent = `${cargaHorariaTotal}h cadastradas`;
    elementos.dashTotalMatriculas.textContent = matriculas.length;
    elementos.dashUltimaMatricula.textContent = ultimaMatricula ? `Ultima em ${formatarData(ultimaMatricula.dataMatricula)}` : "Sem registro recente";

    renderizarAlunosPorCurso();
    renderizarResumoRelatorios(alunosAtivos, alunosInativos, cargaHorariaTotal);
    renderizarUltimasMatriculas();
}

function renderizarAlunosPorCurso() {
    if (!cursos.length) {
        elementos.dashAlunosPorCurso.innerHTML = `<p class="text-muted mb-0">Cadastre cursos para visualizar a distribuicao de alunos.</p>`;
        return;
    }

    const maiorQuantidade = Math.max(...cursos.map(curso => alunos.filter(aluno => aluno.cursoId === curso.id).length), 1);

    elementos.dashAlunosPorCurso.innerHTML = cursos.map(curso => {
        const totalAlunos = alunos.filter(aluno => aluno.cursoId === curso.id).length;
        const percentualBarra = calcularPercentual(totalAlunos, maiorQuantidade);
        const percentualTotal = calcularPercentual(totalAlunos, alunos.length);

        return `
            <div class="course-progress">
                <div class="d-flex justify-content-between gap-3">
                    <span>${escaparHtml(curso.nome)}</span>
                    <strong>${totalAlunos} aluno${totalAlunos === 1 ? "" : "s"}</strong>
                </div>
                <div class="progress" role="progressbar" aria-label="${escaparHtml(curso.nome)}" aria-valuenow="${percentualTotal}" aria-valuemin="0" aria-valuemax="100">
                    <div class="progress-bar" style="width: ${percentualBarra}%"></div>
                </div>
                <small class="text-muted">${percentualTotal}% do total de alunos</small>
            </div>
        `;
    }).join("");
}

function renderizarResumoRelatorios(alunosAtivos, alunosInativos, cargaHorariaTotal) {
    const cursoMaisAlunos = cursos
        .map(curso => ({
            nome: curso.nome,
            total: alunos.filter(aluno => aluno.cursoId === curso.id).length
        }))
        .sort((a, b) => b.total - a.total)[0];

    const disciplinaMaiorCarga = disciplinas
        .slice()
        .sort((a, b) => Number(b.cargaHoraria || 0) - Number(a.cargaHoraria || 0))[0];

    const itens = [
        {
            rotulo: "Alunos ativos",
            valor: `${alunosAtivos} de ${alunos.length}`,
            detalhe: `${calcularPercentual(alunosAtivos, alunos.length)}% da base cadastrada`
        },
        {
            rotulo: "Alunos inativos",
            valor: alunosInativos,
            detalhe: "Use este dado para acompanhamento de evasao ou desligamentos"
        },
        {
            rotulo: "Curso com mais alunos",
            valor: cursoMaisAlunos && cursoMaisAlunos.total ? cursoMaisAlunos.nome : "Sem alunos vinculados",
            detalhe: cursoMaisAlunos && cursoMaisAlunos.total ? `${cursoMaisAlunos.total} aluno${cursoMaisAlunos.total === 1 ? "" : "s"}` : "Cadastre alunos para gerar o ranking"
        },
        {
            rotulo: "Maior carga horaria",
            valor: disciplinaMaiorCarga ? disciplinaMaiorCarga.nome : "Sem disciplinas",
            detalhe: disciplinaMaiorCarga ? `${disciplinaMaiorCarga.cargaHoraria}h de ${cargaHorariaTotal}h totais` : "Cadastre disciplinas para calcular"
        }
    ];

    elementos.dashResumoRelatorios.innerHTML = itens.map(item => `
        <div class="list-group-item report-item">
            <span>${escaparHtml(item.rotulo)}</span>
            <strong>${escaparHtml(item.valor)}</strong>
            <small>${escaparHtml(item.detalhe)}</small>
        </div>
    `).join("");
}

function renderizarUltimasMatriculas() {
    const ultimas = matriculas
        .slice()
        .sort((a, b) => new Date(b.dataMatricula) - new Date(a.dataMatricula))
        .slice(0, 5);

    if (!ultimas.length) {
        elementos.dashUltimasMatriculas.innerHTML = `<tr><td colspan="4" class="text-center py-4">Nenhuma matricula realizada.</td></tr>`;
        return;
    }

    elementos.dashUltimasMatriculas.innerHTML = ultimas.map(matricula => `
        <tr>
            <td>${escaparHtml(matricula.alunoNome)}</td>
            <td>${escaparHtml(matricula.disciplinaNome)}</td>
            <td>${formatarData(matricula.dataMatricula)}</td>
            <td>${escaparHtml(matricula.status)}</td>
        </tr>
    `).join("");
}

function abrirAbaPorSeletor(seletor) {
    const botao = document.querySelector(`[data-bs-target="${seletor}"]`);
    if (!botao) return;

    bootstrap.Tab.getOrCreateInstance(botao).show();
}

async function salvarMatricula(evento) {
    evento.preventDefault();

    const botao = elementos.matriculaForm.querySelector("button[type='submit']");
    definirCarregando(botao, true, "Matriculando...");

    try {
        await requisicao(`${apiBase}/matriculas`, {
            method: "POST",
            body: JSON.stringify({
                alunoId: Number(elementos.matriculaAlunoId.value),
                disciplinaId: Number(elementos.matriculaDisciplinaId.value)
            })
        });

        mostrarMensagem("Matrícula realizada com sucesso.");
        elementos.matriculaForm.reset();
        await carregarMatriculas();
        renderizarDashboard();
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    } finally {
        definirCarregando(botao, false);
    }
}

function abrirExclusaoAluno(id) {
    const aluno = alunos.find(item => item.id === id);
    if (!aluno) return;

    exclusaoAtual = { tipo: "aluno", id };
    elementos.exclusaoTexto.textContent = "Deseja excluir este aluno?";
    elementos.registroExclusaoNome.textContent = `${aluno.nome} - matrícula ${aluno.matricula}`;
    modalExclusao.show();
}

function abrirExclusaoCurso(id) {
    const curso = cursos.find(item => item.id === id);
    if (!curso) return;

    exclusaoAtual = { tipo: "curso", id };
    elementos.exclusaoTexto.textContent = "Deseja excluir este curso?";
    elementos.registroExclusaoNome.textContent = `${curso.codigo} - ${curso.nome}`;
    modalExclusao.show();
}

function abrirExclusaoDisciplina(id) {
    const disciplina = disciplinas.find(item => item.id === id);
    if (!disciplina) return;

    exclusaoAtual = { tipo: "disciplina", id };
    elementos.exclusaoTexto.textContent = "Deseja excluir esta disciplina?";
    elementos.registroExclusaoNome.textContent = `${disciplina.codigo} - ${disciplina.nome}`;
    modalExclusao.show();
}

function abrirExclusaoMatricula(id) {
    const matricula = matriculas.find(item => item.id === id);
    if (!matricula) return;

    exclusaoAtual = { tipo: "matricula", id };
    elementos.exclusaoTexto.textContent = "Deseja excluir esta matrícula?";
    elementos.registroExclusaoNome.textContent = `${matricula.alunoNome} - ${matricula.disciplinaNome}`;
    modalExclusao.show();
}

async function confirmarExclusao() {
    if (!exclusaoAtual) return;

    definirCarregando(elementos.btnConfirmarExclusao, true, "Excluindo...");

    try {
        if (exclusaoAtual.tipo === "aluno") {
            await requisicao(`${apiBase}/alunos/${exclusaoAtual.id}`, { method: "DELETE" });
            mostrarMensagem("Aluno excluído com sucesso.");
            await carregarAlunos();
        } else if (exclusaoAtual.tipo === "curso") {
            await requisicao(`${apiBase}/cursos/${exclusaoAtual.id}`, { method: "DELETE" });
            mostrarMensagem("Curso excluído com sucesso.");
            await carregarCursos();
        } else if (exclusaoAtual.tipo === "disciplina") {
            await requisicao(`${apiBase}/disciplinas/${exclusaoAtual.id}`, { method: "DELETE" });
            mostrarMensagem("Disciplina excluída com sucesso.");
            await carregarDisciplinas();
        } else {
            await requisicao(`${apiBase}/matriculas/${exclusaoAtual.id}`, { method: "DELETE" });
            mostrarMensagem("Matrícula excluída com sucesso.");
            await carregarMatriculas();
        }

        renderizarDashboard();
        modalExclusao.hide();
        exclusaoAtual = null;
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    } finally {
        definirCarregando(elementos.btnConfirmarExclusao, false);
    }
}

async function atualizarDados() {
    definirCarregando(elementos.btnAtualizar, true, "Atualizando...");

    try {
        await carregarDadosHome();
        mostrarMensagem("Dados atualizados.");
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    } finally {
        definirCarregando(elementos.btnAtualizar, false);
    }
}

// ====================Eventos
elementos.loginForm.addEventListener("submit", login);
elementos.btnLogout.addEventListener("click", logout);
elementos.btnAtualizar.addEventListener("click", atualizarDados);
elementos.btnCancelar.addEventListener("click", limparFormulario);
elementos.form.addEventListener("submit", salvarAluno);
elementos.cursoForm.addEventListener("submit", salvarCurso);
elementos.btnCancelarCurso.addEventListener("click", limparFormularioCurso);
elementos.disciplinaForm.addEventListener("submit", salvarDisciplina);
elementos.btnCancelarDisciplina.addEventListener("click", limparFormularioDisciplina);
elementos.matriculaForm.addEventListener("submit", salvarMatricula);
elementos.buscaAluno.addEventListener("input", renderizarTabelaAlunos);
elementos.filtroCurso.addEventListener("change", renderizarTabelaAlunos);
elementos.filtroStatus.addEventListener("change", renderizarTabelaAlunos);
elementos.btnConfirmarExclusao.addEventListener("click", confirmarExclusao);
document.querySelectorAll("[data-ir-aba]").forEach(botao => {
    botao.addEventListener("click", () => abrirAbaPorSeletor(botao.dataset.irAba));
});

// =============Verifica se o usuário já está logado ao carregar a página
if (token) {
    iniciarHome();
} else {
    mostrarTelaLogin();
}
