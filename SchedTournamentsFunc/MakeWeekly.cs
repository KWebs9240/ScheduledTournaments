using System;
using ChallongeApiHelper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using ChallongeEntities.Tournament;

namespace SchedTournamentsFunc
{
    public static class MakeWeekly
    {
        [FunctionName("MakeWeekly")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            ChallongeHttpHelper.setAuthorizationHeader("Chimp9240", "mLeoIg8SM17KTYrGbWazH24YgKcGhHqIAK7W3F8m");
            var test = ChallongeHttpHelper.Get("tournaments.json");



            //TournamentCreation tournamentToCreate = new TournamentCreation()
            //{
            //    name = "Kyle Testing",
            //    url = Guid.NewGuid().ToString().Replace("-", ""),
            //    tournament_type = TournamentConstants.TournamentType.SingleElimination,
            //    open_signup = true,
            //    hold_third_place_match = false,
            //    pts_for_game_win = 0.0m,
            //    pts_for_game_tie = 0.0m,
            //    pts_for_match_win = 1.0m,
            //    pts_for_match_tie = 0.5m,
            //    pts_for_bye = 1.0m,
            //    swiss_rounds = 0,
            //    @private = false,
            //    ranked_by = TournamentConstants.RankedBy.MatchWins,
            //    show_rounds = true,
            //    hide_forum = false,
            //    sequential_pairings = false,
            //    rr_pts_for_game_win = 0.0m,
            //    rr_pts_for_game_tie = 0.0m,
            //    rr_pts_for_match_win = 0.0m,
            //    rr_pts_for_match_tie = 0.0m
            //};

            //var moreTesting = ChallongeHttpHelper.Post("tournaments.json", tournamentToCreate);
        }
    }
}
