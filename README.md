# DotNetMVC_Github_Api

Projeto construido com o .Net Framework de um MVC que conecta à API oficial do Github, buscando os 100 principais repositórios das linguagens C#, Java, JavaScript, Python e Ruby, e armazenando todos os dados para consulta em um banco de dados SQLServer.

## Instalação
Inicialmente antes de rodar o projeto, é necessário estar com o SQLServer instalado na máquina, rodando na porta padrão e adicionar a string de conexão no arquivo appsettings.json.

```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=localhost;Database=GithubRepos;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```
Para facilitar essa parte, foi criado um arquivo de docker-compose que sobe o SQLServer na porta padrão. Para acessá-lo, basta aceessar o terminal na raiz do projeto e rodas os comandos:
```bash
 cd '.\Solution Items\Dockers\sqlserver\'
docker-compose up -d
cd ..\..\..\
```
Depois dessa etapa concluida, é necessario rodar o comando de migração para criar no banco de dados as tabelas necessárias para o projeto. Para isso, basta rodar o comando:
```bash
cd .\Api\
dotnet ef database update
```
Agora está tudo pronto para rodar o projeto. Basta rodar o comando:
```bash
dotnet run
```
Agora basta acessar o endereço http://localhost:5140/ e a aplicação estará rodando.

## Usando as APIs
A aplicação tem 4 endpoints para serem acessados:
- `[GET] /languages`: Retorna as linguagens disponíveis para serem buscadas
- `[PATCH] /repositories`: Busca os repositórios do Github e armazena no banco de dados (esse endpoint demora cerca de 30 segundos para rodar completamente pois ele acessa a Api do github e salva ao todo 500 repositório, 100 para cada uma das 5 linguagens)
- `[GET] /repositories`: Retorna os repositórios salvos no banco de dados de forma paginada (permite filtar por linguagem e escolher a quantidade de itens por pagina e a pagina)
- `[GET] /repositories/{id}`: Retorna um repositório específico pelo id

## Detalhamento da Solução

### Arquitetura do Projeto
O projeto foi construido com a arquitetura Clean Arquitecture, onde temos a divisão de camadas de acordo com a responsabilidade de cada uma.

A camada de Api é a apresentação da aplicação, onde temos os controladores que vão retornar os dados por meio de endpoints.

A camada de Aplicação responsável por conter as regras de negócio da aplicação coordenando o fluxo de dados.

a camada de Domínio é responsável por conter  os modelos de dados. 

A camada de Repositórios é responsável por conter as implementações de acesso a dados.

Abaixo é possível visualizar a estrutura do projeto:
```plaintext
DotNetMVC_Github_Api
├───Api
│   ├───Configuration
│   ├───Controllers
│   ├───Migrations
├───Application
│   ├───Commands
│   │   ├───GetRepositoriesByFilters
│   │   ├───GetRepositoryById
│   │   ├───Results
│   │   └───UpdateAllData
│   ├───OutputPorts
│   └───Services
│       └───Github
├───Domain
│   ├───GithubRepo
├───Repositories
│   ├───Models
├───Solution Items
│   └───Dockers
│       └───sqlserver
└───UnitTests
    ├───Commands
    ├───Fixtures
```

### Analise Inicial da API do Github
Na documentação do Github API, temos uma biblioteca oficial para o .Net chamado de Octokit. A vantagem de utilizar essa biblioteca é de já ter mapeado os modelos de dados dentro do projeto. Para buscar os Repositorios do Github temos a linguagem de programação como um enum, o Repositório como uma classe que dentro dela tem duas classes referenciadas (a Licença e o Usuário). Dessa forma a solução proposta repete a lógica de mapeamento dos modelos de dados do Github.

### Mapeamento das Classes e do Domínio
O Domínio em questão é o Repositório Github (chamado de GithubRepo no projeto) e ele é composto por 3 classes e um enum: a classe de Repositório, a classe de Licença, a classe de Usuário e o enum das linguagens. A classe de Repositório é a classe principal que contém todas as informações do repositório, a classe de Licença contém as informações da licença do repositório e a classe de Usuário contém as informações do usuário que criou o repositório. 

Abaixo é possível visualzar o mapeamento de todos esses itens:
```csharp
    public enum LanguageEnum
    {
        CSharp,
        Java,
        JavaScript,
        Python,
        Ruby
    }

    public class GithubRepo
    {
        public Guid Id { get; private set; }
        public long GitId { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public string Description { get; private set; }
        public string Homepage { get; private set; }
        public LanguageEnum Language { get; private set; }
        public string Url { get; private set; }
        public string HtmlUrl { get; private set; }
        public string CloneUrl { get; private set; }
        public string GitUrl { get; private set; }
        public string SshUrl { get; private set; }
        public string SvnUrl { get; private set; }
        public string MirrorUrl { get; private set; }
        public string ArchiveUrl { get; private set; }
        public string NodeId { get; private set; }
        public bool IsTemplate { get; private set; }
        public bool Private { get; private set; }
        public bool Fork { get; private set; }
        public int ForksCount { get; private set; }
        public int StargazersCount { get; private set; }
        public string DefaultBranch { get; private set; }
        public int OpenIssuesCount { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string Parent { get; private set; }
        public string Source { get; private set; }
        public List<string> Topics { get; private set; }
        public Licence? License { get; private set; }
        public Owner Owner { get; private set; }
    }

    public class Licence
    {
        public Guid Id { get; private set; }
        public string NodeId { get;  private set; }
        public string Key { get; private set; }
        public string Name { get; private set; }
        public string SpdxId { get; private set; }
        public string Url { get; private set; }
        public bool Featured { get; private set; }
    } 

    public class Owner
    {
        public Guid Id { get; private set; }
        public long GitId { get; private set; }
        public string Login { get; private set; }
        public string AvatarUrl { get; private set; }
        public string Url { get; private set; }
        public string HtmlUrl { get;  private set; }
        public string Type { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
```

### Relacionamento entre as Classes
Aqui é importante lembrar que o repositório não tem uma relação one-to-one com a licença e o usuário, pois um repositório pode não ter uma licença e um usuário pode ter vários repositórios. Dessa forma, o relacionamento entre as classes é de um para muitos, onde um repositório tem uma licença e um usuário, mas um usuário pode ter vários repositórios e uma licença vários repositórios.

Sendo assim, na camade de Repositório, temos essa configuração no SqlDbContext.cs:
```csharp
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<GithubRepo>()
			.HasOne(x => x.License)
			.WithMany()
			.HasForeignKey(x => x.LicenseId);

		modelBuilder.Entity<GithubRepo>()
			.HasOne(x => x.Owner)
			.WithMany()
			.HasForeignKey(x => x.OwnerId);
	}
```
E no arquivo GithubRepoRepository.cs temos a validação de que a cada inserção de um repositório é verificado se a licença e o usuário já estão no banco de dados, caso não estejam, são inseridos e caso estejam o Repositório recebe seus Ids como chaves:
```csharp
	public async Task AddAsync(GithubRepo repository)
        {
            var model = Map(repository);

            if(model.License is not null)
            {
                var existingLicence = await _context.Licences.FirstOrDefaultAsync(l => l.Key == repository.License.Key);
                if (existingLicence != null)
                {
                    _context.Entry(existingLicence).State = EntityState.Unchanged;
                    model.License = existingLicence;
                }
            }

            var existingOwner = await _context.Owners.FirstOrDefaultAsync(o => o.GitId == repository.Owner.GitId);
            if (existingOwner != null)
            {
                _context.Entry(existingOwner).State = EntityState.Unchanged;
                model.Owner = existingOwner;
            }

            await _context.Repositories.AddAsync(model);
            await _context.SaveChangesAsync();
        }
```
### Comandos
Dentro da camada de Aplicação temos 3 comandos: UpdateAllData, GetRepositoriesByFilters e GetRepositoryById. 

O comando UpdateAllData é de extrema importância, nele temos o passoo inicial de limpar todos os itens do banco de dados, buscar pela Api do Github os 100 principais repositórios de cada uma das 5 linguagens e salvar no banco de dados. Ao final desse comando temos ao todo 500 repositórios, 461 usuários e 17 licenças salvos no banco sql. Ele pode ser executado chamando o endpoint [PATCH] /repositories.

O comando GetRepositoriesByFilters é responsável por buscar os repositórios salvos no banco de dados de forma paginada, permitindo filtrar por linguagem e escolher a quantidade de itens por pagina e a pagina. Nele tem a validação de limite máximo de itens por página, permitindo o máximo de 100 para não sobrecarregar as conexões da aplicação com o banco de dados (extremamente importante em um ambiente de produção). Ele pode ser executado chamando o endpoint [GET] /repositories.

O comando GetRepositoryById é responsável por buscar um repositório específico pelo id. Ele pode ser executado chamando o endpoint [GET] /repositories/{id}.

### Testes Unitários
Na camada de UnitTests temos testes unitários para os comandos de UpdateAllData, GetRepositoriesByFilters e GetRepositoryById. Para rodar os testes basta rodar o comando:
```bash
cd .\UnitTests\
dotnet test
```