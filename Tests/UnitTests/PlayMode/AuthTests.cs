using System.Collections;
using Cysharp.Threading.Tasks;
using Extensions.Console.Interfaces;
using FluentAssertions;
using Meta.Auth;
using NSubstitute;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.UnitTests.PlayMode
{
    [TestFixture]
    public class AuthTests
    {
        [UnityTest]
        public IEnumerator When_NotSignedIn_Then_SignInAnonymouslyWithUnity()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       //Arrange
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var unityServiceInitializer = new UnityServiceInitializer(mockLogger);
                                       var authService = new UnityAuthService(mockLogger);

                                       // Initialize Unity Services
                                       await unityServiceInitializer.InitializeAsync();

                                       // Act
                                       await authService.SignInAnonymouslyAsync();

                                       // Assert
                                       authService.IsSignedIn.Should().BeTrue("the user should be signed in anonymously");
                                       mockLogger.Received(1).Log(Arg.Is<string>(s => s.Contains("Player signed in successfully!")), "SignInAnonymouslyAsync");
                                   });

        [UnityTest]
        public IEnumerator When_AlreadySignedIn_Then_AttemptSignInAnonymouslyDoesNothing()
            => UniTask.ToCoroutine(async () =>
                                   {
                                       //Arrange
                                       var mockLogger = Substitute.For<IDebugLogger>();
                                       var unityServiceInitializer = new UnityServiceInitializer(mockLogger);
                                       var authService = new UnityAuthService(mockLogger);

                                       // Initialize Unity Services
                                       await unityServiceInitializer.InitializeAsync();

                                       // Act
                                       await authService.SignInAnonymouslyAsync();
                                       await authService.SignInAnonymouslyAsync();

                                       // Assert
                                       authService.IsSignedIn.Should().BeTrue("the user should be signed in anonymously");
                                       mockLogger.Received(1).Log(Arg.Is<string>(s => s.Contains("Player already signed in!")), "SignInAnonymouslyAsync");
                                   });
    }
}