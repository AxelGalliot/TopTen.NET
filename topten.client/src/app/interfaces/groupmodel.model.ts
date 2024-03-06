import { PlayerModel } from "./playermodel.model";
import { ThemeModel } from "./thememodel.model";

export interface GroupModel {
  id: string,
  state: string,
  hostId: string,
  captainId: string,
  playerList: PlayerModel[],
  remainingHp: number,
  roundNumber: number,
  theme: ThemeModel,
  sortedAnswers: string[],
}
