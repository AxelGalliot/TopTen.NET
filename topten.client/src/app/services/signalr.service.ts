import { Injectable, signal } from '@angular/core'
import * as signalR from "@microsoft/signalr"
import { GroupModel } from '../interfaces/groupmodel.model'
import { Store } from '@ngrx/store'
import { Observable } from 'rxjs'
import { update } from '../stores/groupstore.actions'

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  data$: Observable<GroupModel>
  private hubConnection: signalR.HubConnection

  constructor(private store: Store<{ group: GroupModel }>) {
    this.data$ = store.select('group')
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7187/topten')
      .build()
  }

  public startConnection = () => {
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public updatedListener = () => {
    this.hubConnection.on('Updated', (data) => {
      this.store.dispatch(update({ data }))
    })
  }

  public errorListener = () => {
    this.hubConnection.on('Error', (data) => {
      console.log(`Error received : ${data}`)
    })
  }

  public createGroupInvoker = (avatar:string, name: string) => {
    this.hubConnection.invoke('creategroup', { name: name, avatar: avatar })
  }

  public joinGroupInvoker = (avatar: string, name: string, groupId: string) => {
    this.hubConnection.invoke('joingroup', { group: groupId, name: name, avatar: avatar })
  }

  public leaveGroupInvoker = (groupId: string) => {
    this.hubConnection.invoke('leavegroup', { groupId })
  }

  public startGameInvoker = (groupId: string) => {
    this.hubConnection.invoke('startgame', groupId)
  }

  public startRoundInvoker = () => {

  }

  public lockAnswerInvoker = () => {

  }

  public unockAnswerInvoker = () => {

  }

  public sortAnswerInvoker = () => {

  }

  public lockSortedAnswerInvoker = () => {

  }

  public returnToLobbyInvoker = () => {

  }
}
