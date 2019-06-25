import React from "react";
import {Container, Content, Text, Header, Button, Left, Body, Title, Icon, Right, View} from "native-base";
import Constants from "expo-constants";
import { HeaderBar } from "../../components/headerBar";

export const Help = (props) => {
    return (
        <Container style={{flex:1}}>
            <HeaderBar navigation={props.navigation} title="Help" showSearch={false} />
            <View style={{
                    alignSelf: 'center',
                    flexDirection: 'row',
                    alignItems: 'center',
                }}>
                <Text>How we calculate your green score and cost</Text>
            </View>
        </Container>
    )
};