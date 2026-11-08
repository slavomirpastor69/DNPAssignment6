using CLI.UI;
using FileRepositories;
using RepositoryContracts;


Console.WriteLine("Start CLI App..");
UserFileRepository userRepository = new UserFileRepository();
CommentFileRepository commentRepository = new CommentFileRepository();
PostFileRepository postRepository = new PostFileRepository();

CliApp cliApp = new CliApp(userRepository, commentRepository,
    postRepository);
await cliApp.StartAsync();
  