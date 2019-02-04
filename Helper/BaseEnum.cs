using System;
namespace BoshokuDemo1.Helper
{
    public enum E_TabItem
    {
        newsfeed = 0,
        activity = 1,
        upload = 2,
        settings = 3
    }
    public enum PushCategory
    {
        announcements = 0,
        activities = 1,
        survey = 2,
        user_interaction = 4,
        multiple_choice = 3
    }
    public enum AnnCategory
    {
        survey = 1,
        activities = 0,
        user_interaction = 4,
        multiple_choice = 3
    }
    public enum WebReadCateogry
    {
        announ = 0,
        activities = 1,

    }
    public enum ActivityCategory
    {
        key,
        category,
        endAt,
        startAt,
        endDate
    }
    public enum Event
    {
        multiple_choice = 0,
        user_interaction = 1

    }

    public enum ListCount
    {
        low = 5,
        mid = 10,
        midHalf = 15,
        high = 20
    }

    public enum MCenter
    {
        dataActivity,
        dataNotify,
        dataSurvey,
        dataUserInteraction,
        dataMultipleChoice,
        notificationReceived,
        question,
        questionKey,
        questionTitle,
        deleteSurveyKey,
        competitionToInteractionKey,
        onResume,
        annMultipleChoice,
        annUserInteraction,
        announToInteractionKey,
        eventToIntereact,
        errorPagePopKey,
        toastKey,
        toastAnimationKey,
        rightSideKey,
        tabClickKey,
        tabListResetKey,
        dataAnnouncementsKey,
        dataActivityAnnounKey

    }
}
