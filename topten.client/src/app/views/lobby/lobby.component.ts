import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { GroupModel } from '../../interfaces/groupmodel.model';
import { selectFilledPlayerList, selectGroupId, selectHost } from '../../stores/groupstore.selector';
import { SignalrService } from '../../services/signalr.service';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrl: './lobby.component.css'
})
export class LobbyComponent {
  group$ = this.store.select(selectGroupId)
  host$ = this.store.select(selectHost)
  playerList$ = this.store.select(selectFilledPlayerList)

  constructor(public signalRService: SignalrService, private store: Store<GroupModel>) { }

  async startGame() {
    this.group$.subscribe(
      res => this.signalRService.startGameInvoker(res)
    )
  }
}
