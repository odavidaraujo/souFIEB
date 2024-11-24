document.addEventListener("DOMContentLoaded", function (event) {
    let qrCodeUsageData = [];
    let currentPage = 0;
    const resultsPerPage = 6;
    var aluno = {};

    function findAlunoById(id) {
        const alunos = qrCodeUsageData.alunos;
        for (let i = 0; i < alunos.length; i++) {
            if (alunos[i].rm === id) {
                aluno = {
                    nome: alunos[i].nome,
                    foto: alunos[i].foto
                }
                return aluno;
            }
        }

        return null;
    }

    var infos = [];
    function fetchInfo() {
        fetch("/informative/getinfo")
            .then(response => {
                if (!response.ok) {
                    throw new Error("Erro na solicitação.");
                }
                return response.json();
            })
            .then(data => {
                infos = data;
                numInfo();

            })
            .catch(error => {
                console.error("Erro ao buscar dados:", error);
            });
    }

    fetchInfo();

    async function numInfo() {
        var lastInformativo = document.getElementById("lastDataInformativoNotifications");

        moment.locale('pt-br');

        var dataDoUltimoInformativo = moment(infos.lastData);
        var dataFormatada = moment(dataDoUltimoInformativo).locale('pt-br').fromNow();
        lastInformativo.textContent = dataFormatada;


    }


    // renderizar a tabela de usuários
    function renderUserTable(qrCodeUsageData) {
        const userTableBody = document.getElementById('userTableBody');

        userTableBody.innerHTML = '';

        const startIndex = currentPage * resultsPerPage;
        const endIndex = startIndex + resultsPerPage;
        const paginatedData = qrCodeUsageData.refeicoes.slice(startIndex, endIndex);
        
        
        paginatedData.forEach((data, index) => {
            
            const HoraFormatada = moment(data.data).format("HH:mm");
            const dataFormatada = moment(data.data).format("DD/MM/YYYY");
            const row = document.createElement('tr');
            row.innerHTML = `
            <td class="align-middle text-center">
                <img src="${findAlunoById(data.idAluno).foto}" class="img-fluid" style="width: 50px; height: 50px; border-radius: 50%;" alt="Usuário 1">
            </td>
            <td>
                <h6 class="mb-0">${findAlunoById(data.idAluno).nome}</h6>
                <h8 class="mb-0">RM: ${data.idAluno} <br></h8>
                <h8 class="mb-0">Tipo: Aluno</h8>
            </td>
            <td>
                ${dataFormatada} <br>
                ${HoraFormatada}
            </td>`

            userTableBody.appendChild(row);
        });
    }

    // Função para atualizar a tabela de usuários e o gráfico de utilização
    function updateData() {
        renderUserTable(qrCodeUsageData);
    }

    // Função para avançar para a próxima página da tabela
    
    function nextPage() {
        const filteredData = qrCodeUsageData.refeicoes;
        const totalPages = Math.ceil(filteredData.length / resultsPerPage);

        if (currentPage < totalPages - 1) {
            currentPage++;
            updateData();

            if (currentPage > 0) {
                var buttons = document.getElementsByClassName("prevPageButton");
                for (var i = 0; i < buttons.length; i++) {
                    buttons[i].classList.remove("disabled");
                }
            }
            if (currentPage == totalPages - 1) {
                var buttons = document.getElementsByClassName("nextPageButton");
                for (var i = 0; i < buttons.length; i++) {
                    buttons[i].classList.add("disabled");
                }
            }
        }
    }

    function prevPage() {
        if (currentPage > 0) {
            currentPage--;
            updateData();

            if (currentPage <= 0) {
                var buttons = document.getElementsByClassName("prevPageButton");
                for (var i = 0; i < buttons.length; i++) {
                    buttons[i].classList.add("disabled");
                }
            }
            if (currentPage >= 0) {
                var buttons = document.getElementsByClassName("nextPageButton");
                for (var i = 0; i < buttons.length; i++) {
                    buttons[i].classList.remove("disabled");
                }
            }
        }
    };

    // Event listener para o botão de próxima página
    const nextPageButton = document.getElementById('nextPageButton');
    nextPageButton.addEventListener('click', nextPage);

    // Event listener para o botão de página anterior
    const prevPageButton = document.getElementById('prevPageButton');
    prevPageButton.addEventListener('click', prevPage);

    function padTo2Digits(num) {
        return num.toString().padStart(2, "0");
    }

    function convertMsToHM(milliseconds) {
        let seconds = Math.floor(milliseconds / 1000);
        let minutes = Math.floor(seconds / 60);
        const hours = Math.floor(minutes / 60);
        seconds = seconds % 60;
        minutes = seconds >= 30 ? minutes + 1 : minutes;
        minutes = minutes % 60;
        return `${padTo2Digits(hours)}h ${padTo2Digits(minutes)}m`;
    }

    function timeToMilliseconds(timeString) {
        const [hours, minutes, seconds] = timeString.split(':').map(Number);
        const totalMilliseconds = (hours * 60 * 60 + minutes * 60 + seconds) * 1000;
        return totalMilliseconds;
    }

    const allData = {
        cafeData: [],
        almocoData: [],
        jantaData: []
    };
    
    function filterRefeicaoDaily() {
        const filteredRefeicao = qrCodeUsageData;

        filteredRefeicao.refeicoes.forEach(refeicao => {

            const horaFormatada = moment(refeicao.data).format("HH:mm");
            const dataFormatada = moment(refeicao.data).format("DD/MM/YYYY");
            const currentDate = moment(new Date()).format("DD/MM/YYYY");
            if (dataFormatada !== currentDate) {
                return false;
            }
            const timestamp = timeToMilliseconds(moment(refeicao.data).format("HH:mm:ss"));
            const quantidade = 1;

            if (refeicao.periodo === "Manhã") {
                const existingRecord = allData.cafeData.find(record => record[0] === timestamp);
                if (existingRecord) {
                    existingRecord[1] += quantidade;
                } else {
                    allData.cafeData.push([timestamp, quantidade]);
                }
                
            } else if (refeicao.periodo === "Tarde") {
                const existingRecord = allData.almocoData.find(record => record[0] === timestamp);
                if (existingRecord) {
                    existingRecord[1] += quantidade;
                } else {
                    allData.almocoData.push([timestamp, quantidade]);
                }

            } else if (refeicao.periodo === "Noite") {
                const existingRecord = allData.jantaData.find(record => record[0] === timestamp);
                if (existingRecord) {
                    existingRecord[1] += quantidade;
                } else {
                    allData.jantaData.push([timestamp, quantidade]);
                }
            }
        });
        containerBar();
        return allData;
    }

    const weekData = {
        data: [],
        others: []
    };
    function filterRefeicaoWeekly() {
        let filteredRefeicao = qrCodeUsageData;

        filteredRefeicao.refeicoes.forEach(refeicao => {
            const dataDaRefeicao = new Date(refeicao.data);
            const currentDate = new Date();
            const dataFormatada = moment(refeicao.data).format("DD/MM/YYYY");
            
            if (
                dataDaRefeicao.getFullYear() >= currentDate.getFullYear() &&
                dataDaRefeicao.getMonth() >= currentDate.getMonth() &&
                moment(refeicao.data).week() >= moment().week()
            ) {
                const daysofweek = ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'];
                const dayofweek = daysofweek[dataDaRefeicao.getDay()];
                /*if (dayofweek == 'Domingo' || dayofweek == 'Sábado') {
                    return false;
                }*/
                var sliced = false;
                var selected = false;

                var numCafe = 0;
                var numAlmoco = 0;
                var numJanta = 0;
                var allNum = 0;

                if (dataFormatada == moment(currentDate).format("DD/MM/YYYY")) {
                    sliced = true;
                    selected = true;
                }

                if (refeicao.periodo === "Manhã") {

                    const existingRecord = weekData.data.find(record => record.date === dataFormatada);
                    if (existingRecord) {
                        existingRecord.y += 1;
                        existingRecord.total += 1;
                        const numsRecord = weekData.others.find(record => record.date === dataFormatada);
                        if (numsRecord) numsRecord.cafe += 1

                    } else {
                        allNum += 1;
                        weekData.data.push({ name: dayofweek, y: 1, sliced: sliced, selected: selected, date: dataFormatada, total: allNum });
                        numCafe += 1;
                        weekData.others.push({ date: dataFormatada, cafe: numCafe, almoco: numAlmoco, janta: numJanta });
                    }

                    
                } else if (refeicao.periodo === "Tarde") {
                    const existingRecord = weekData.data.find(record => record.date === dataFormatada);
                    if (existingRecord) {
                        existingRecord.y += 1;
                        existingRecord.total += 1;
                        const numsRecord = weekData.others.find(record => record.date === dataFormatada);
                        if (numsRecord) numsRecord.cafe += 1

                    } else {
                        allNum += 1;
                        weekData.data.push({ name: dayofweek, y: 1, sliced: sliced, selected: selected, date: dataFormatada, total: allNum });
                        numAlmoco += 1;
                        weekData.others.push({ date: dataFormatada, cafe: numCafe, almoco: numAlmoco, janta: numJanta });
                    }

                } else if (refeicao.periodo === "Noite") {
                    const existingRecord = weekData.data.find(record => record.date === dataFormatada);
                    if (existingRecord) {
                        existingRecord.y += 1;
                        existingRecord.total += 1;

                        const numsRecord = weekData.others.find(record => record.date === dataFormatada);
                        if (numsRecord) numsRecord.cafe += 1

                    } else {
                        allNum += 1;
                        weekData.data.push({ name: dayofweek, y: 1, sliced: sliced, selected: selected, date: dataFormatada, total: allNum });
                        numJanta += 1;
                        weekData.others.push({ date: dataFormatada, cafe: numCafe, almoco: numAlmoco, janta: numJanta });
                        
                    }


                }
                
            }
        })
        containerPie();
        return weekData;
    }

    function containerBar() {
       
        Highcharts.chart("containerbar", {
            chart: {
                type: "spline"
            },
            title: {
                text: "Resumo Diário"
            },
            subtitle: {
                text: null
            },
            xAxis: {
                title: {
                    text: "Tempo (horas)"
                },
                tickInterval: 2 * 60 * 60 * 1000,
                labels: {
                    formatter: function () {
                        return convertMsToHM(this.value).split(" ")[0];
                    }
                }
            },
            yAxis: {
                title: {
                    text: "Quantidade"
                },
                labels: {
                    format: "{text}"
                }
            },
            tooltip: {
                useHTML: true,
                formatter: function () {
                    return `
                <div>
                    <strong>Hora:</strong> ${convertMsToHM(this.x)}<br>
                    <strong>Quantidade:</strong> ${this.y}
                </div>
            `;
                }
            },
            plotOptions: {
                spline: {
                    dataLabels: {
                        enabled: false
                    },
                    marker: {
                        enabled: false
                    }
                }
            },
            series: [
                {
                    name: "Café",
                    data: allData.cafeData
                },
                {
                    name: "Almoço",
                    data: allData.almocoData
                },
                {
                    name: "Janta",
                    data: allData.jantaData
                }
            ]
        });
    }
    function containerPie() {

        Highcharts.chart('containerPie', {
            colors: ['#01BAF2', '#f6fa4b', '#FAA74B', '#baf201', '#f201ba'],
            chart: {
                type: 'pie'
            },
            title: {
                text: 'Resumo Semanal'
            },
            tooltip: {
                valueSuffix: '%'
            },
            subtitle: {
                text: null
            },
            tooltip: {
                useHTML: true,
                formatter: function () {
                    const dataFormatada = this.point.date;
                    const dataEntry = weekData.others.find(entry => entry.date === dataFormatada);

                    const total = weekData.data.reduce((sum, entry) => sum + entry.total, 0);

                    weekData.data.forEach(entry => {
                        entry.percentage = (entry.total / total) * 100;
                    });                    
                    
                    return `
                <div>
                    <strong>Café:</strong> ${dataEntry.cafe}
                    <strong>Almoço:</strong> ${dataEntry.almoco}
                    <strong>Janta:</strong> ${dataEntry.janta}<br>
                    <strong>Porcentagem:</strong> ${this.point.percentage}%<br>
                    <strong>Data:</strong> ${dataEntry.date} 
                </div>
                `;
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}: {point.percentage:.1f}%'
                    },
                    showInLegend: true
                }
            },
            series: [
                {
                    name: 'Percentage',
                    colorByPoint: true,
                    data: weekData.data
                }
            ]
        });
    }

    function fetchRefeicoes() {
        fetch("/Statistics/GetRefeicoes")
            .then(response => {
                if (!response.ok) {
                    throw new Error("Erro na solicitação.");
                }
                return response.json();
            })
            .then(data => {
                qrCodeUsageData = data;
                filterRefeicaoDaily();
                renderUserTable(qrCodeUsageData);
                filterRefeicaoWeekly();
            })
            .catch(error => {
                console.error("Erro ao buscar dados:", error);
            });
    }
    fetchRefeicoes();


});