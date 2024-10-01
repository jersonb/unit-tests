# Testes de Unidade

## Para que servem?

### 1. Documentação

> Como primeiro código cliente do código produtivo, os testes são uma excelente fonte de conhecimento para desenvolvedores se familiarizar com a implementação real.

### 2. Garantia contra regressões

> Refatorações e implementação de novas funcionalidades fazem parte do ciclo de vida de um software. Testes garantem que funcionalidades antigas sigam funcionando da forma esperada.

### 3. Facilitação no code review

> A verificação da cobertura de testes pode indicar trechos de código com baixa qualidade e facilitar o entendimento de regras de negócio complexas.

### 4. Multiplicação de conhecimento

> Colegas podem alternar entre o desenvolvimento de código de testes e código produtivo e dessa forma ambos entendem oque estão fazendo.

## Unidade ou Unitário??

> Ao traduzir o termo `Unit Test` do inglês para o português, podemos obter `Teste Unitário` ou `Teste de Unidade`, ambos são aceitos, no entanto o termo unitário remete a quantidade, já o termo Unidade remete a unicidade, remetendo a qualidade e estado único, desta forma sendo este o termo mais adequado.

## Unidade de código

- Por padrão os métodos públicos são considerados as unidades de um software.

- Porém dependendo da arquitetura do software e dos acordos do time, a noção de unidade pode ser modificada, podendo ser uma classe, por exemplo nos softwares que utilizam DDD e até mesmo um serviço inteiro em alguns casos de microsserviços.

> ***OBS:*** senários que fogem dos padrões devem ser muito bem documentados.

## Qualidade de Teste de Unidade

- Os testes devem ser independentes de serviços externos, código servidor ou código cliente. Desta forma devemos utilizar mocks e fakes para simular estes serviços.

- As asserções devem ser feitas nos retornos dos métodos ou modificação de informação caso não tenha retorno, ou se determinado recurso de serviço "mocado" foi acionado.

- Cada teste deve ter o mínimo de asserções.
  
- Deve existir o máximo de testes possível que cubra fluxo do código.
  
- Se os testes de unidade estiverem difíceis de serem escritos, esse pode ser um indicativo de que o código possui baixa qualidade.

### Mock

> Instância que **representa** código servidor onde os retornos podem ser configurados dentro do próprio teste. Geralmente a partir de Interfaces. Desta forma não é necessário a utilização de código real.

## Fake

> Instância de uma classe concreta que substitui objetos ou serviços necessários para a realização dos testes. Geralmente implementando a interface do serviço.

## Serviço

> Uma boa prática e excelente maneira de manter o código limpo. Possibilita o isolamento de códigos que podem ser facilmente substituídos. Em linguagens compiladas como Java e C# isso é possível através de abstrações (classes abstratas e interfaces).

## Quantidade de Testes

- Idealmente todos os fluxos devem ser testados, inclusive tratamentos de exceção.
  
- Cada regra de negócio descrita no código deve possuir um teste direcionado.

- Deve ser priorizada a cobertura de testes com lógica e tomadas de decisão.
