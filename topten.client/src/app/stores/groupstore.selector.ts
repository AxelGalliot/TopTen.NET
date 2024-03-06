import { createFeatureSelector, createSelector } from '@ngrx/store'
import { GroupModel } from '../interfaces/groupmodel.model'
import { PlayerModel } from '../interfaces/playermodel.model'

const selectGroup = createFeatureSelector<GroupModel>('group')

export const selectGroupId = createSelector(
  selectGroup,
  (group: GroupModel) => group.id
)

export const selectState = createSelector(
  selectGroup,
  (group: GroupModel) => group.state
)

export const selectHost = createSelector(
  selectGroup,
  (group: GroupModel) => group.hostId
)

export const selectCaptain = createSelector(
  selectGroup,
  (group: GroupModel) => group.captainId
)

export const selectPlayerList = createSelector(
  selectGroup,
  (group: GroupModel) => group.playerList
)

export const selectFilledPlayerList = createSelector(
  selectGroup,
  (group: GroupModel) => {
    let array: (PlayerModel | null)[] = [].constructor(8)
    for (let i = 0; i <= group.playerList?.length ?? 0 - 1; i++)
        array[i] = group.playerList[i]
    return array;
  }
)

export const selectRoundNumber = createSelector(
  selectGroup,
  (group: GroupModel) => group.roundNumber
)

export const selectRemainingHp = createSelector(
  selectGroup,
  (group: GroupModel) => group.remainingHp
)

export const selectFilledHpList = createSelector(
  selectGroup,
  (group: GroupModel) => {
    let array: (boolean)[] = []
    for (let i = 0; i <= (group.playerList?.length ?? 0) - 1; i++)
      array[i] = i + 1 <= group.remainingHp
    return array;
  }
)

export const selectThemeArray = createSelector(
  selectGroup,
  (group: GroupModel) => {
    let array: ({ type: string, content: string })[] = []

    let smallestIndex = group.theme.content.indexOf('{-}')
    let biggestIndex = group.theme.content.indexOf('{+}')

    if (smallestIndex > 0)
      array.push({ type: "text", content: group.theme.content.substring(0, smallestIndex) })

    array.push({ type: "small", content: group.theme.smallest })

    if (biggestIndex > (smallestIndex + 3))
      array.push({ type: "text", content: group.theme.content.substring(smallestIndex + 3, biggestIndex) })

    array.push({ type: "big", content: group.theme.biggest })

    if (biggestIndex + 3 - group.theme.content.length > 0)
      array.push({ type: "text", content: group.theme.content.substring(biggestIndex + 3) })

    return array;
  })

  export const selectWeight = createSelector(
    selectGroup,
    (group: GroupModel) => group.
  )
)
