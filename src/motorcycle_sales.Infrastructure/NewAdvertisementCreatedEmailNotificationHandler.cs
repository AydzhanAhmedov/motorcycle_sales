
using System.Linq;
using System.Text.Encodings.Web;
using MediatR;
using Microsoft.AspNetCore.Identity;
using motorcycle_sales.Core.Entities;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.Core.Interfaces;
using motorcycle_sales.Core.ProjectAggregate.Events;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Core.ProjectAggregate.Handlers;

public class NewAdvertisementCreatedEmailNotificationHandler : INotificationHandler<NewAdvertisementCreatedEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly IUserSearchFilterService _userSearchFilterService;
    private readonly IAppLogger<NewAdvertisementCreatedEmailNotificationHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<Advertisement> _advertisementRepository;

    public NewAdvertisementCreatedEmailNotificationHandler(IEmailSender emailSender
        , IUserSearchFilterService userSearchFilterService
        , IAppLogger<NewAdvertisementCreatedEmailNotificationHandler> logger
        , UserManager<ApplicationUser> userManager
        , IRepository<Advertisement> advertisementRepository
        )
    {
        _emailSender = emailSender;
        _userSearchFilterService = userSearchFilterService;
        _logger = logger;
        _userManager = userManager;
        _advertisementRepository = advertisementRepository;
    }
    public async Task Handle(NewAdvertisementCreatedEvent notification, CancellationToken cancellationToken)
    {
        var advertisementSpecification = new AdvertisementWithDetailsSpecification(notification.Advertisement.Id);
        var advertisement = await _advertisementRepository.GetBySpecAsync(advertisementSpecification);

        List<UserSearchFilter> userSearchFilters = await _userSearchFilterService.GetMatchingFiltersForAdvertisement(advertisement);

        // Select unique User ids
        string []usersId = userSearchFilters.GroupBy(filter => filter.UserId)
            .Select(grp => grp.First().UserId)
            .ToArray();
        
        foreach (string userId in usersId)
        {
            var email = await GenerateEmailForAdvertisementAsync(userId, advertisement);
            await _emailSender.SendEmailAsync(email.Receiver, "motorcycles@email.bg" , email.Subject, email.Body);
        }
    }

    private async Task<Email> GenerateEmailForAdvertisementAsync(string userId, Advertisement advertisement)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            _logger.LogWarning("User not found");
            return null;
        }

        // TODO replace hardcoded url
        string urlAdvertisement = "http://localhost:57678/advertisement/view/" + advertisement.Id;

        string emailBody = $"<h3>New advertisement matching your filter has been added.<h3>" +
            "<h4> Motorcycle: " +advertisement.Brand.Name + " " + advertisement.Model.Name + ", Price: " + advertisement.Price + " EUR </h4>" +
            $" Check out the advertisement details by <a href='{HtmlEncoder.Default.Encode(urlAdvertisement)}'>clicking here</a>.";

        var email = new Email()
        {
            Receiver = user.Email,
            Subject = "New advertisement added " + advertisement.Brand.Name + " " + advertisement.Model.Name,
            Body = emailBody
        };

        return email;
    }
}
