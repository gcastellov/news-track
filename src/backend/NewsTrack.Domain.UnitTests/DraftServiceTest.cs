using System;
using System.Threading.Tasks;
using Moq;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;
using NewsTrack.Domain.Services;
using Xunit;
using FluentAssertions;

namespace NewsTrack.Domain.UnitTests
{
    
    public class DraftServiceTest
    {
        private Mock<IDraftRepository> _draftRepositoryMock;
        private Mock<IContentRepository> _contentRepositoryMock;
        private Mock<IDraftRelationshipRepository> _draftRelationshipRepositoryMock;
        private DraftService _draftService;

        public DraftServiceTest()
        {
            _draftRepositoryMock = new Mock<IDraftRepository>();
            _contentRepositoryMock = new Mock<IContentRepository>();
            _draftRelationshipRepositoryMock = new Mock<IDraftRelationshipRepository>();
            _draftService = new DraftService(
                _draftRepositoryMock.Object,
                _contentRepositoryMock.Object,
                _draftRelationshipRepositoryMock.Object);
        }

        [Fact]
        public async Task WhenSaving_ThenStoreDraftAndContent()
        {
            const string body = "this is the content of the draft";

            var draft = new Draft
            {
                Uri = new Uri("http://some.website.com/entry.html"),
                Title = "The title",
                Paragraphs = new [] { "paragraph1", "paragraph2" },
                Picture = new Uri("http://some.website.com/img1.png"),
                Tags = new [] { "tag1", "tag2" },
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "username"
                }
            };

            _draftRepositoryMock.Setup(m => m.Save(draft)).Returns(Task.CompletedTask);
            _contentRepositoryMock.Setup(m => m.Save(It.IsAny<Content>()))
                .Callback((Content c) =>
                {
                    c.Id.Should().Be(draft.Id);
                    c.Body.Should().Be(body);
                })
                .Returns(Task.CompletedTask);

            await _draftService.Save(draft, body);

            _draftRepositoryMock.Verify(m => m.Save(draft), Times.Once);
            _contentRepositoryMock.Verify(m => m.Save(It.IsAny<Content>()), Times.Once);
        }

        [Fact]
        public async Task WhenSettingRelationship_ThenStoreRelationship()
        {
            var id = Guid.NewGuid();
            var items = new[]
            {
                new DraftRelationshipItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Title 1",
                    Url = new Uri("http://uri.com/entry1.html")
                },
                new DraftRelationshipItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Title 2",
                    Url = new Uri("http://uri.com/entry2.html")
                }
            };

            _draftRelationshipRepositoryMock.Setup(m => m.SetRelationship(It.IsAny<DraftRelationship>()))
                .Callback((DraftRelationship r) =>
                {
                    r.Id.Should().Be(id);
                    r.Relationship.Should().BeEquivalentTo(items);
                })
                .Returns(Task.CompletedTask);

            _draftRepositoryMock.Setup(m => m.SetRelationship(id, items.Length)).Returns(Task.CompletedTask);

            await _draftService.SetRelationships(id, items);

            _draftRelationshipRepositoryMock.Verify(m => m.SetRelationship(It.IsAny<DraftRelationship>()), Times.Once);
            _draftRepositoryMock.Verify(m => m.SetRelationship(id, items.Length), Times.Once);
        }
    }
}