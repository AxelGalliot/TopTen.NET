import { HttpClientModule } from '@angular/common/http'
import { NgModule, isDevMode } from '@angular/core'
import { LottieModule } from 'ngx-lottie'
import { BrowserModule } from '@angular/platform-browser'
import { AppRoutingModule } from './app-routing.module'
import { AppComponent } from './app.component'
import { StoreModule } from '@ngrx/store'
import { groupReducer } from './stores/groupstore.reducer'
import { StoreDevtoolsModule } from '@ngrx/store-devtools'
import { HomeComponent } from './views/home/home.component'
import { AvatarpickerComponent } from './components/avatarpicker/avatarpicker.component'
import { SvgComponent } from './components/svg/svg.component'
import { TextinputComponent } from './components/textinput/textinput.component'
import { CTAComponent } from './components/cta/cta.component';
import { TabsComponent } from './components/tabs/tabs.component';
import { LobbyComponent } from './views/lobby/lobby.component'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AvatarComponent } from './components/avatar/avatar.component';
import { RoundReadyComponent } from './views/round-ready/round-ready.component';
import { HpBarComponent } from './components/hp-bar/hp-bar.component';
import { PlayerBarComponent } from './components/player-bar/player-bar.component';
import { RoundSetupComponent } from './components/round-setup/round-setup.component';
import { ThemeComponent } from './components/theme/theme.component';
import { JaugeComponent } from './components/jauge/jauge.component'

export function playerFactory() {
  return import ('lottie-web')
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AvatarpickerComponent,
    SvgComponent,
    TextinputComponent,
    CTAComponent,
    TabsComponent,
    LobbyComponent,
    AvatarComponent,
    RoundReadyComponent,
    HpBarComponent,
    PlayerBarComponent,
    RoundSetupComponent,
    ThemeComponent,
    JaugeComponent,
    LottieModule.forRoot({player: playerFactory}),
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    StoreModule.forRoot({ group: groupReducer}),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: !isDevMode() }),
    //HomeComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
