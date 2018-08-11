using System;
using ChallongeApiHelper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using ChallongeEntities.Tournament;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Mail;

namespace SchedTournamentsFunc
{
    public static class MakeWeekly
    {
        [FunctionName("MakeWeekly")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            ChallongeHttpHelper.setAuthorizationHeader("Chimp9240", "apiKey");
            var getResponse = ChallongeHttpHelper.BasicGet("tournaments.json");

            List<TournamentHolderGarbage> tournamentList = JsonConvert.DeserializeObject<List<TournamentHolderGarbage>>(getResponse);

            var mostRecent = tournamentList.Where(x => x.tournament.name.Contains("QDAL")).OrderByDescending(x => x.tournament.updated_at).First();

            int weekNum = int.Parse(mostRecent.tournament.name.Split(' ').Last());
            weekNum++;

            TournamentCreation tournamentToCreate = new TournamentCreation()
            {
                //name = $"QDAL Friday Fray - Week {weekNum.ToString()}",
                name = "Kyle Test",
                url = Guid.NewGuid().ToString().Replace("-", ""),
                tournament_type = TournamentConstants.TournamentType.SingleElimination,
                open_signup = true,
                hold_third_place_match = false,
                pts_for_game_win = 0.0m,
                pts_for_game_tie = 0.0m,
                pts_for_match_win = 1.0m,
                pts_for_match_tie = 0.5m,
                pts_for_bye = 1.0m,
                swiss_rounds = 0,
                @private = false,
                ranked_by = TournamentConstants.RankedBy.MatchWins,
                show_rounds = true,
                hide_forum = false,
                sequential_pairings = false,
                rr_pts_for_game_win = 0.0m,
                rr_pts_for_game_tie = 0.0m,
                rr_pts_for_match_win = 0.0m,
                rr_pts_for_match_tie = 0.0m
            };

            var postResponse = ChallongeHttpHelper.Post("tournaments.json", tournamentToCreate);

            TournamentHolderGarbage postGarbage = JsonConvert.DeserializeObject<TournamentHolderGarbage>(postResponse);

            string signUpUrl = postGarbage.tournament.sign_up_url;

            //Email whoever cares
            MailMessage mail = new MailMessage
            {
                //TODO: need to change this to a real @qbsol.com email
                From = new MailAddress("weeklypingpong@gmail.com")
            };

            string[] emailRecipients = "kyle_webster@qbsol.com".Split(';');

            foreach (var recipient in emailRecipients)
            {
                mail.To.Add(recipient);
            }

            //TODO: need to change to the real @qbsol.com creds
            SmtpClient client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true
            };
            client.Credentials = new System.Net.NetworkCredential("weeklypingpong@gmail.com", "password");

            mail.Subject = "Ping Pong Email";
            mail.IsBodyHtml = true;
            mail.Body = $"Tournament Created - QDAL Friday Fray - Week {weekNum.ToString()} <br /> Signup Link: {signUpUrl}";

            client.Send(mail);
            log.Info("IAN Batch Monitor ran succesfully, found issues, and sent emails!");
        }
    }
}
