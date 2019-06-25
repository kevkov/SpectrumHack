import React, {useState} from 'react';
import {Body, Button, Container, Header, Icon, Left, Right, Title} from "native-base";
import { Map } from '../../components/map';
import Constants from "expo-constants";
import JourneyContext from '../../context/JourneyContext';
import { JourneySettings } from '../../domain/types';

export const Route = (props) => {
    const [showPollution, togglePollution] = useState(() => true);
    const [showSchools, toggleSchools] = useState(() => true);
    const [startTime, setStartTime] = useState(() => '12:00');
    const [showSearch, toggleSearch] = useState(false);

    const context: JourneySettings = {
        showPollution,
        showSchools,
        startTime,
        togglePollution,
        toggleSchools,
        setStartTime
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
