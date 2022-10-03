# Primeiro Projeto - Pedido de Creditações

## Tema


O primeiro projeto terá como objetivo a criação de uma solução digital para o Regulamento de Creditações Académicas, Experiências Profissionais e Outra Formação do Instituto Universitário da Maia.

Este regulamento foi aprovado no dia 18 de dezembro de 2018, e homologado pelo Reitor do Instituto Universitário da Maia no mesmo dia.

Neste regulamento o aluno pode obter dois tipos de creditação, sendo que existe uma divisão entre os tipos de creditação que o aluno deseja obter. Se este pretender obter creditação de Competências Académicas, exceto para o caso dos Cursos de Especialização Tecnológica, o processo de tramitação vai seguir o seguinte processo (sendo que os prazos são interrompidos durante o mês de agosto):

1.	No prazo de 5 dias úteis o Coordenador de Curso convoca o Júri;
2.	No prazo de 20 dias úteis o Júri delibera sobre o pedido de creditação, e o Coordenador de Curso remete o processo ao Presidente do Conselho Científico, incluindo no processo o original da ata do Júri;
3.	No prazo de 5 dias úteis o Presidente do Conselho Científico remete o processo aos Serviços da Secretaria com conhecimento ao Reitor.

Caso o aluno pretenda obter creditação no contexto de Cursos de Especialização Tecnológica, Experiência Profissional ou Outra Formação, o processo de tramitação segue o seguinte processo (sendo que os prazos são interrompidos durante o mês de agosto):

1.	No prazo de 5 dias úteis o Coordenador de Curso convoca o Júri;
2.	No prazo de 20 dias úteis o Júri delibera sobre o pedido de creditação, e o Coordenador de Curso remete o processo ao Presidente do Conselho Científico, incluindo no processo o original da ata do Júri;
3.	No prazo de 5 dias úteis o Presidente do Conselho Científico remete aos Serviços de Secretaria com o conhecimento ao Reitor.
Com este projeto, o pretendido passa por inovar e simplificar este processo manual, permitindo ter um controlo visual do processo e garantir a segurança de todos os documentos envolvidos neste processo.


## Ferramentas de apoio à construção da solução

Para desenvolver este tema foi decidido que para se poder ter um acompanhamento visual dos processos, o Trello é a ferramenta indicada para assegurar que os prazos são cumpridos de forma cómoda e para que nenhum documento se perca durante o processo. 
O Trello organiza os projetos em boards e por temas, logo, pode ter-se a perceção do que está a ser feito, por quem, e o estado do mesmo. Posto isto, existem três boards, um para os processos dos coordenadores de curso, outro para os diretores de departamento e, por fim, um para o concelho científico. Os boards não necessitam obrigatoriamente ter estes nomes, pois na solução vão ser utilizados os seus identificadores:

Ao usar esta ferramenta consegue-se, não só, ter um controlo visual de um processo, como também fazer lembretes aos associados das entregas que têm pendentes, e ainda garantir a segurança de todos os documentos.
Além de termos uma representação visual do processo, vamos ter também um responsável por orquestrar todos os processos de tramitações, o Camunda BPMN, sendo responsável por finalizar ou iniciar uma tramitação consoante o estado de uma card no Trello.
Por fim, para fazer o envio da notificação para a secretária, além de se recorrer ao RabbitMQ para enfileirar a mensagem para o serviço de notificações a processar, vai ser utilizado um serviço de envio de e-mails.

## Vídeo


[![IMAGE ALT TEXT](http://img.youtube.com/vi/THPOhAIXoMk/0.jpg)](https://youtu.be/THPOhAIXoMk "Primeiro Projeto")


Autor: Carlos Campos
