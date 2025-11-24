UPDATE public."Setting"
SET "Value" = 'https://www.sarayetel.com'
WHERE "Name" IN (
    'storeinformationsettings.facebooklink',
    'storeinformationsettings.twitterlink',
    'storeinformationsettings.youtubelink',
    'storeinformationsettings.instagramlink'
);
