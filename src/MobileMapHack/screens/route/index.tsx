import React, {useState} from 'react';
import {Body, Button, Container, Header, Icon, Left, Right, Title} from "native-base";
import { Map } from '../../components/map';
import Constants from "expo-constants";
import JourneyContext from '../../context/JourneyContext';
import {Journey, JourneyPlannerParams, JourneySettings} from '../../domain/types';

export const Route = (props) => {
    const [journey, setJourney] = useState<Journey>(null);
    const [showPollution, togglePollution] = useState(() => false);
    const [showSchools, toggleSchools] = useState(() => false);
    const [startTime, setStartTime] = useState(() => '12:00');
    const [showSearch, toggleSearch] = useState(false);
    const [journeyPlannerParams, setJourneyPlannerParams] = useState<JourneyPlannerParams>(null);

    const context: JourneySettings = {
        journey,
        showPollution,
        showSchools,
        journeyPlannerParams,
        startTime,
        setJourney,
        togglePollution,
        toggleSchools,
        setStartTime,
        setJourneyPlannerParams
    };

    return (
        <JourneyContext.Provider value={context}>
            <Container style={{flex:1}}>
                <Header style={{paddingTop: Constants.statusBarHeight}}>
                    <Left>
                        <Button
                            transparent
                            onPress={() => props.navigation.openDrawer()}>
                            <Icon name="menu"/>
                        </Button>
                    </Left>
                    <Body>
                        <Title>Routes</Title>
                    </Body>
                    <Right>
                        <Icon name="search" type="MaterialIcons" style={{color: "#FFFFFF"}}
                            onPress={() => toggleSearch(!showSearch)}
                        />
                    </Right>
                </Header>
                <Map {...props} showSearch={showSearch} />
            </Container>
        </JourneyContext.Provider>
    )
};
