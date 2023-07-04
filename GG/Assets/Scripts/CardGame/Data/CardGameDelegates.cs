public delegate void GameAction();

public delegate void CardAction(CardInfo card);

public delegate void CardsAction(CardInfo[] cards);

public delegate void EnemySkillAction(SkillInfo skill, Enemy enemy);

public delegate void EnemyAbilityAction(AbilityInfo ability, Enemy enemy);

public delegate void EnemyEnrageAction(EnrageInfo enrage, Enemy enemy);

public delegate int GetPower(CardInfo card);

public delegate CardInfo ChangeCardAction(CardInfo card, int value);

public delegate CardInfo ChangeCard(CardInfo card);

public delegate void GameEventAction(CardGameManager manager, CardInfo card);

public delegate void GameEvent(CardGameManager manager);

public delegate void AttackEvent(Participant attacker, Participant defender, int damage);

public delegate void DefendEvent(CardInfo card, ref int damage);