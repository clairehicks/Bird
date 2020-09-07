using JetBrains.Annotations;

public static class LevelStrings
{
    public static class LevelOne
    {
        public const string Welcome = "Hello little bird, ready to fly?";
        public const string Flap = "Press \"UP\" arrow to flap harder.";
        public const string FlapLess = "Excellent, now press \"DOWN\" arrow to flap less.";
        public const string Forward = "Now try \"SPACE\" to move forwards. \n You can use this to fly or walk.";
        public const string Turn = "How about we try turning using \"LEFT\" and \"RIGHT\" arrows?";
        public const string Complete = "Well done! \nYou now have half a minute to practice flying before the next level.";
    }

    public static class LevelTwo
    {
        public const string Welcome = "Oh look, your person left the bird seed out.";
        public const string GetBox = "Fly to the box and use \"B\" to pick it up with your beak.";
        public const string DropBox = "This box is heavy, find a nice clear spot and use \"N\" to drop it.";
        public const string Eat = "Oh look, seed fell out! Land next to the box and use \"B\" to eat the seed.";
        public const string FoodBar = "You need energy to fly, eating seed fills up your energy bar";
        public const string FoodBar2 = "There is another box in this room, make sure you eat again before your energy runs out. ";
        public const string Eat2 = "The faster you flap, the sooner you need to eat again.";
        public const string Complete = "Naughty bird! I'm going to have to start hiding your food.";
    }

    public static class LevelThree
    {
        public const string Welcome = "You are feeling hungry again, luckily your person has hidden food in the top drawers.";
        public const string OpenCage = "First things first, use your beak to open your cage door.";
        public const string LeaveCage = "Now fly through the open door to explore the bedroom.";
        public const string OpenDrawer = "Fly to the drawers under your cage and use \"C\" to open the top left drawer with your claws.";
        public const string Eat = "A box of seed, go ahead and eat it.";
        public const string Again = "You're still hungry. Use your claws again to open the top middle drawer then eat some more seed.";
        public const string Complete = "That's better! You are now ready to leave the bedroom";
    }

    public const string Failure = "Oh no, you are too hungry, let's try this again.";

    //todo add level four bathing and add bathing stats to five
    public static class LevelFive
    {
        public const string Welcome = "Now that you know all about being a bird, you are ready to explore the house.";
        public const string Food = "Remember to look for hidden food to keep up your energy.";
        public const string CompleteStats = "You explored for {0} and ate {1} boxes of food on this adventure";
    }
}
